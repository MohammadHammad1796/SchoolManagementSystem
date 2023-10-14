using Microsoft.AspNetCore.Builder;

namespace SchoolManagementSystem.Shared.Exceptions;

public static class SchoolExceptionExtensions
{
	public static void UseExceptionHandling(this IApplicationBuilder application)
	{
		application.UseMiddleware<ExceptionHandlingMiddleware>();
	}
}