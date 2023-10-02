namespace Authorization.Controllers;

using Microsoft.AspNetCore.Mvc;


[Route("test")]
public class TestController : Controller
{
	[HttpGet("test")]
	public IActionResult Test()
	{
		return Ok($"{AppDomain.CurrentDomain.FriendlyName} test succeeded.");
	}
}