using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementSystem.Shared.Extensions;

public static class ControllerExtensions
{
	public static StatusCodeResult ServerError(this Controller _)
		=> new(StatusCodes.Status500InternalServerError);

	public static bool ValidateWithFluent<T>(this Controller controller,
		AbstractValidator<T> validator, T resource)
	{
		if (resource is not null)
		{
			var validationResult = validator.Validate(resource);
			if (validationResult.Errors.Any())
				validationResult.Errors
					.ForEach(failure => controller.ModelState
						.AddModelError(failure.PropertyName, failure.ErrorMessage));
		}

		return controller.ModelState.IsValid;
	}
}