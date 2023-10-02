using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SchoolManagementSystem.Authorization.Controllers.Resources;
using SchoolManagementSystem.Authorization.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SchoolManagementSystem.Authorization.Services;

public class AccountService : IAccountsService
{
	private readonly ApplicationDbContext _dbContext;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly SignInManager<ApplicationUser> _signInManager;
	private readonly JwtSettings _jwtSettings;
	private readonly TokenValidationParameters _tokenValidationParameters;

	public AccountService(
		ApplicationDbContext dbContext,
		UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager,
		IOptionsMonitor<JwtSettings> jwtSettings,
		TokenValidationParameters tokenValidationParameters)
	{
		_dbContext = dbContext;
		_userManager = userManager;
		_signInManager = signInManager;
		_tokenValidationParameters = tokenValidationParameters;
		_jwtSettings = jwtSettings.CurrentValue;
	}

	public bool IsAccountLocked(ApplicationUser user)
	{
		return user.LockoutEnabled && user.LockoutEnd > DateTime.UtcNow;
	}

	public async Task AccessFailedAsync(ApplicationUser user)
	{
		await _signInManager.UserManager.AccessFailedAsync(user);
		if (user.AccessFailedCount != 0) return;

		user.LockoutEnabled = true;
		var userRefreshTokens = await _dbContext.RefreshTokens.Where(rt => rt.UserId == user.Id).ToListAsync();
		userRefreshTokens.RemoveAll(_ => true);
	}

	public async Task<JwtResource> GenerateJwtAsync(ApplicationUser user)
	{
		var roles = await _userManager.GetRolesAsync(user);

		var claims = new List<Claim>
		{
			new(ClaimTypes.Email, user.Email),
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new("roles", string.Join(',', roles))
		};

		var jwtTokenHandler = new JwtSecurityTokenHandler();

		var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

		var accessExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.LifeTime);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = accessExpiresAt,
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
				SecurityAlgorithms.HmacSha256Signature),
			Issuer = _jwtSettings.Issuer,
			Audience = _jwtSettings.Audience
		};

		var token = jwtTokenHandler.CreateToken(tokenDescriptor);
		var accessToken = jwtTokenHandler.WriteToken(token);

		var refreshExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshLifeTime);
		var refreshTokenValue = Guid.NewGuid().ToString();
		var refreshTokenSalt = GenerateSalt();
		var refreshTokenHash = HashRefreshToken(refreshTokenValue, refreshTokenSalt);
		var refreshToken = new RefreshToken(
			token.Id,
			user.Id,
			refreshExpiresAt,
			refreshTokenSalt,
			refreshTokenHash);

		await _dbContext.RefreshTokens.AddAsync(refreshToken);

		var jwt = new JwtResource(accessToken, refreshTokenValue, refreshExpiresAt);
		if (user.AccessFailedCount <= 0) return jwt;

		await _signInManager.UserManager.ResetAccessFailedCountAsync(user);
		user.LockoutEnabled = false;
		user.LockoutEnd = null;

		return jwt;
	}

	public async Task<Result<RefreshToken>> GetRefreshTokenIfValidAsync(string accessToken, string refreshToken)
	{
		var jwtTokenHandler = new JwtSecurityTokenHandler();
		ClaimsPrincipal principal;
		SecurityToken validatedToken;
		var result = new Result<RefreshToken>();
		try
		{
			principal = jwtTokenHandler.ValidateToken(accessToken, _tokenValidationParameters,
				out validatedToken);
		}
		catch (Exception)
		{
			return result;
		}

		if (validatedToken is not JwtSecurityToken jwtSecurityToken)
			return result;

		var isValidAlgorith = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
			StringComparison.InvariantCultureIgnoreCase);
		if (!isValidAlgorith)
			return result;

		if (validatedToken.ValidTo > DateTime.UtcNow)
			return result;

		var tokenId = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

		var storedRefreshToken = await _dbContext.RefreshTokens
			.SingleOrDefaultAsync(rt => rt.JwtId == tokenId);
		if (storedRefreshToken == null)
			return result;

		if (storedRefreshToken.TokenHash != HashRefreshToken(refreshToken, storedRefreshToken.Salt))
			return result;

		if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
			return result;

		result.Succeeded = true;
		result.ObjectResult = storedRefreshToken;
		return result;
	}

	private static string GenerateSalt()
	{
		var salt = new byte[16];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(salt);
		return Convert.ToBase64String(salt);
	}

	private static string HashRefreshToken(string refreshToken, string salt)
	{
		using var hmac = new HMACSHA256(Convert.FromBase64String(salt));
		var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(refreshToken));
		return Convert.ToBase64String(computedHash);
	}
}