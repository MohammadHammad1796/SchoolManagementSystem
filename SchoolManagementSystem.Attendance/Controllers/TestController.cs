using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementSystem.Attendance.Controllers;

[Route("test")]
public class TestController : Controller
{
	[HttpGet("test")]
	public IActionResult Test()
	{
		return Ok($"{AppDomain.CurrentDomain.FriendlyName} test succeeded.");
	}

	[HttpGet("testAuth")]
	[Authorize(Roles = "Adminasd,Admin")]
	public IActionResult TestAuth()
	{
		return Ok($"{AppDomain.CurrentDomain.FriendlyName} authorize test succeeded.");
	}
}