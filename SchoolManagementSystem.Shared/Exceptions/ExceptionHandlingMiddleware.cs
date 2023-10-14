using Microsoft.AspNetCore.Http;
using System.Net;

namespace SchoolManagementSystem.Shared.Exceptions;

internal class ExceptionHandlingMiddleware
{
	private readonly RequestDelegate _next;

	public ExceptionHandlingMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception)
		{
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
		}
	}
}