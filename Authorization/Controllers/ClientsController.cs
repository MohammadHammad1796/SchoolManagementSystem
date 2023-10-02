using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Shared.Auth;

namespace Authorization.Controllers;

[Route("clients")]
public class ClientsController : Controller
{
	[HttpGet("validateJwt")]
	[Authorize]
	public IActionResult ValidateJwt()
	{
		var claims = User.Claims.ToList();
		var result = new List<ClaimResource>();
		claims.ForEach(c =>
			result.Add(new ClaimResource(c.Type, c.Value)));
		return Ok(result);
	}
}