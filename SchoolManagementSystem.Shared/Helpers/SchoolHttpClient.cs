using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SchoolManagementSystem.Shared.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SchoolManagementSystem.Shared.Helpers;

public class SchoolHttpClient : ISchoolHttpClient
{
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly string _serviceName;

	public SchoolHttpClient(
		IHttpClientFactory httpClientFactory,
		IHttpContextAccessor httpContextAccessor,
		IConfiguration configuration)
	{
		_httpClientFactory = httpClientFactory;
		_httpContextAccessor = httpContextAccessor;
		_serviceName = configuration["ServiceName"];
	}

	public async Task<Result<T>> GetResultAsync<T>(string url)
	{
		var result = await GetAsync<Result<T>>(url);
		return result ?? new Result<T>(false);
	}

	public async Task<T> GetAsync<T>(string url) where T : new()
	{
		using var httpClient = _httpClientFactory.CreateClient();
		var authHeader = _httpContextAccessor.HttpContext!.Request.Headers["Authorization"].ToString();
		var token = authHeader!.Replace("Bearer ", "");

		httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		httpClient.DefaultRequestHeaders.Add("x-services-name", _serviceName);
		var response = await httpClient.GetAsync(url);
		if (!response.IsSuccessStatusCode)
			return default;

		var obj = await response.Content.ReadAsStringAsync();
		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};
		var result = JsonSerializer.Deserialize<T>(obj, options);
		return result;
	}
}