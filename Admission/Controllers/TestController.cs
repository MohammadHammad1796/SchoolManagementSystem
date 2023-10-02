using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Shared.Auth;

namespace Admission.Controllers;

[Route("test")]
public class TestController : Controller
{
	[HttpGet("test")]
	public IActionResult Test()
	{
		return Ok($"{AppDomain.CurrentDomain.FriendlyName} test succeeded.");
	}

	[HttpGet("testAuth")]
	[Authorize(Roles = Roles.AdminOrStudent)]
	public IActionResult TestAuth()
	{
		return Ok($"{AppDomain.CurrentDomain.FriendlyName} authorize test succeeded.");
	}
}