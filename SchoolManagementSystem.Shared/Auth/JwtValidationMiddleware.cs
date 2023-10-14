using Microsoft.AspNetCore.Http;
using SchoolManagementSystem.Shared.Helpers;
using System.Security.Claims;

namespace SchoolManagementSystem.Shared.Auth;

internal class JwtValidationMiddleware
{
	private readonly RequestDelegate _next;
	private readonly string _serviceUrl;

	public JwtValidationMiddleware(RequestDelegate next, string serviceUrl)
	{
		_next = next;
		_serviceUrl = serviceUrl;
	}

	public async Task Invoke(HttpContext context, ISchoolHttpClient schoolHttpClient)
	{
		if (context.Request.Headers.ContainsKey("Authorization"))
		{
			var validateJwtUrl = $"{_serviceUrl}clients/validateJwt";
			var authenticationResult = await schoolHttpClient.GetResultAsync<List<ClaimResource>>(validateJwtUrl);
			if (authenticationResult.Succeeded)
			{
				var claims = new List<Claim>();
				authenticationResult.ObjectResult!.ForEach(c => claims.Add(new Claim(c.Type, c.Value)));

				var identity = new ClaimsIdentity(claims, "custom", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
				context.User = new ClaimsPrincipal(identity);
			}
		}
		await _next(context);
	}
}