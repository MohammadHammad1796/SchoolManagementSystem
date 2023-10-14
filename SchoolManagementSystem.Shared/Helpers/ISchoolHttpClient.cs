using SchoolManagementSystem.Shared.Models;

namespace SchoolManagementSystem.Shared.Helpers;

public interface ISchoolHttpClient
{
	Task<Result<T>> GetResultAsync<T>(string url);

	Task<T> GetAsync<T>(string url) where T : new();
}