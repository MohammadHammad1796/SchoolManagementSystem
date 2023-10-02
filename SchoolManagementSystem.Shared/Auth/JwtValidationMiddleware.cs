using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace SchoolManagementSystem.Shared.Auth;

internal class JwtValidationMiddleware
{
	private readonly RequestDelegate _next;
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly string _serviceUrl;

	public JwtValidationMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory, string serviceUrl)
	{
		_next = next;
		_httpClientFactory = httpClientFactory;
		_serviceUrl = serviceUrl;
	}

	public async Task Invoke(HttpContext context)
	{

		if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
		{
			await _next(context);
			return;
		}


		var token = authHeader.ToString()!.Replace("Bearer ", "");

		using var httpClient = _httpClientFactory.CreateClient();
		httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await httpClient.GetAsync($"{_serviceUrl}clients/validateJwt");

		if (!response.IsSuccessStatusCode)
		{
			await _next(context);
			return;
		}

		var obj = await response.Content.ReadAsStringAsync();
		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};
		var claimResources = JsonSerializer.Deserialize<List<ClaimResource>>(obj, options);

		var claims = new List<Claim>();
		claimResources?.ForEach(c => claims.Add(new Claim(c.Type, c.Value)));

		var identity = new ClaimsIdentity(claims, "custom", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
		context.User = new ClaimsPrincipal(identity);

		await _next(context);
	}
}