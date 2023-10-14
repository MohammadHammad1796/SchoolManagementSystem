using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Authorization.Models;
using SchoolManagementSystem.Authorization.Services;
using SchoolManagementSystem.Shared.Auth;
using SchoolManagementSystem.Shared.Models;
using System.Security.Claims;

namespace SchoolManagementSystem.Authorization.Controllers;

[Route("clients")]
public class ClientsController : Controller
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly ApplicationDbContext _dbContext;
	private readonly IAccountsService _accountsService;

	public ClientsController(UserManager<ApplicationUser> userManager,
		ApplicationDbContext dbContext,
		IAccountsService accountsService)
	{
		_userManager = userManager;
		_dbContext = dbContext;
		_accountsService = accountsService;
	}

	[HttpGet("validateJwt")]
	[AuthorizeService(AppServices.Admission, AppServices.Courses, AppServices.Attendance)]
	[Authorize]
	public IActionResult ValidateJwt()
	{
		var claims = User.Claims.ToList();
		var result = new Result<List<ClaimResource>>(true, new List<ClaimResource>());
		claims.ForEach(c =>
			result.ObjectResult!.Add(new ClaimResource(c.Type, c.Value)));
		return Ok(result);
	}

	[HttpGet("getMyId")]
	[AuthorizeService(AppServices.Admission)]
	[Authorize(Roles = Roles.NoRoleOrStudent)]
	public async Task<IActionResult> GetMyId()
	{
		var email = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
		var user = await _userManager.FindByEmailAsync(email);
		var result = new Result<string>(true, user.Id);
		return Ok(result);
	}

	[HttpGet("makeUserStudent")]
	[AuthorizeService(AppServices.Admission)]
	[Authorize(Roles = Roles.NoRole)]
	public async Task<IActionResult> MakeUserStudent()
	{
		var email = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
		var user = await _userManager.FindByEmailAsync(email);
		if (user is null)
			return Unauthorized();

		await _accountsService.RemoveFromRoleAsync(user, Roles.NoRole);
		await _userManager.AddToRoleAsync(user, Roles.Student);

		await _dbContext.SaveChangesAsync();
		return Ok(new Result<int>(true));
	}
}