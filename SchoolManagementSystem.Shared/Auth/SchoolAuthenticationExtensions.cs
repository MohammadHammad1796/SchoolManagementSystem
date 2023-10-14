using Microsoft.AspNetCore.Builder;

namespace SchoolManagementSystem.Shared.Auth;

public static class SchoolAuthenticationExtensions
{
	public static void UseSchoolAuthentication(this IApplicationBuilder application, string serviceUrl)
	{
		application.UseMiddleware<JwtValidationMiddleware>(serviceUrl);
	}
}