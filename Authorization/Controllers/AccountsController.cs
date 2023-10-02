﻿using Authorization.Controllers.Resources;
using Authorization.Extensions;
using Authorization.Models;
using Authorization.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Authorization.Controllers;

[Route("accounts")]
public class AccountsController : Controller
{
	private readonly ApplicationDbContext _dbContext;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IAccountsService _accountsService;

	public AccountsController(ApplicationDbContext dbContext,
		UserManager<ApplicationUser> userManager,
		IAccountsService accountsService)
	{
		_dbContext = dbContext;
		_userManager = userManager;
		_accountsService = accountsService;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterResource resource)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var user = await _userManager.FindByNameAsync(resource.Email);
		if (user != null)
		{
			ModelState.AddModelError(nameof(resource.Email).ToCamelCaseName(), "user already registered.");
			return BadRequest(ModelState);
		}

		user = new ApplicationUser()
		{
			Email = resource.Email,
			UserName = resource.Email,
			NormalizedUserName = resource.Email.ToUpper(),
			NormalizedEmail = resource.Email.ToUpper()
		};

		await _userManager.CreateAsync(user, resource.Password);
		var affectedRows = await _dbContext.SaveChangesAsync();

		affectedRows += await _dbContext.SaveChangesAsync();
		if (affectedRows == 0)
			return StatusCode(StatusCodes.Status500InternalServerError);

		return NoContent();
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginResource resource)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

		var user = await _userManager.FindByEmailAsync(resource.Email);
		if (user is null)
			return Unauthorized();

		var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, resource.Password);
		if (!isPasswordCorrect)
		{
			if (_accountsService.IsAccountLocked(user))
				return Unauthorized(new UnauthorizedResource("Your account is locked. Lock end at: {0}", user.LockoutEnd));

			await _accountsService.AccessFailedAsync(user);

			await _dbContext.SaveChangesAsync();
			return Unauthorized();
		}

		if (_accountsService.IsAccountLocked(user))
			return Unauthorized(new UnauthorizedResource("Your account is locked. Lock end at: {0}", user.LockoutEnd));

		var jwt = await _accountsService.GenerateJwtAsync(user);

		try
		{
			await _dbContext.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			return Ok(ex.Message);
		}
		return Ok(jwt);
	}


	[HttpPost("refreshAccessToken")]
	public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshTokenRequestResource refreshTokenRequestResource)
	{
		var validateResult = await _accountsService.GetRefreshTokenIfValidAsync(refreshTokenRequestResource.AccessToken,
			refreshTokenRequestResource.RefreshToken);
		if (!validateResult.Succeeded)
			return Unauthorized();

		_dbContext.RefreshTokens.Remove(validateResult.ObjectResult);

		var user = await _userManager.FindByIdAsync(validateResult.ObjectResult.UserId);
		if (user is null)
			return Unauthorized();

		if (_accountsService.IsAccountLocked(user))
			return Unauthorized(new UnauthorizedResource("Your account is locked. Lock end at: {0}", user.LockoutEnd));

		var jwt = await _accountsService.GenerateJwtAsync(user);

		await _dbContext.SaveChangesAsync();

		return Ok(jwt);
	}

	[Authorize]
	[HttpGet("logout")]
	public async Task<IActionResult> Logout()
	{
		var userEmail = this.User.Claims.Single(c => c.Type == ClaimTypes.Email).Value;
		var user = await _userManager.FindByEmailAsync(userEmail);
		if (user is null)
			return NoContent();

		var accessTokenId = User.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
		var refreshToken = await _dbContext.RefreshTokens.SingleOrDefaultAsync(rt => rt.JwtId == accessTokenId);
		if (refreshToken is not null)
		{
			_dbContext.RefreshTokens.Remove(refreshToken);
			await _dbContext.SaveChangesAsync();
		}

		return NoContent();
	}
}