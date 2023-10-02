using System.ComponentModel.DataAnnotations;

namespace Authorization.Controllers.Resources;

public class LoginResource
{
	[Required(ErrorMessage = "Please enter your e-mail")]
	[RegularExpression(@"^[a-zA-Z0-9_\.-]+@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$", ErrorMessage = "Email is not a valid e-mail address")]
	public string Email { get; set; }

	[Required(ErrorMessage = "Please enter your password")]
	[DataType(DataType.Password)]
	public string Password { get; set; }
}