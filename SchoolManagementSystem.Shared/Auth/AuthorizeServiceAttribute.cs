using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SchoolManagementSystem.Shared.Auth;

public class AuthorizeServiceAttribute : ActionFilterAttribute
{
	private readonly ICollection<string> _servicesName;

	public AuthorizeServiceAttribute(params string[] servicesName)
	{
		_servicesName = servicesName;
	}

	public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		if (context.HttpContext.Request.Headers.TryGetValue("x-services-name", out var name))
		{
			if (_servicesName.Contains(name))
				return base.OnActionExecutionAsync(context, next);
		}

		context.Result = new UnauthorizedResult();
		return Task.CompletedTask;
	}
}