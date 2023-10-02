using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Authorization.Controllers.Resources;

public class RegisterResource
{
	[Required(ErrorMessage = "Please enter your e-mail")]
	[RegularExpression(@"^[a-zA-Z0-9_\.-]+@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$", ErrorMessage = "Email is not a valid e-mail address")]
	public string Email { get; set; }

	[Required(ErrorMessage = "Please enter your password")]
	[DataType(DataType.Password)]
	[MinLength(8)]
	[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{1,}$", ErrorMessage = "{0} should contain the following at least: one capital letter, one lower letter, one number, one non alphabet character.")]
	public string Password { get; set; }
}