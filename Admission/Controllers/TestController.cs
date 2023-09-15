using Microsoft.AspNetCore.Mvc;

namespace Admission.Controllers;

[Route("test")]
public class TestController : Controller
{
	[HttpGet("test")]
	public IActionResult Test()
	{
		return Ok($"{AppDomain.CurrentDomain.FriendlyName} test succeeded.");
	}
}