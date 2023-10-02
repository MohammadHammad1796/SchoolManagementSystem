using Microsoft.AspNetCore.Identity;

namespace Authorization.Models;

public class ApplicationUserRole : IdentityUserRole<string>
{
	public string Id { get; set; }

	public ApplicationUser User { get; set; }

	public ApplicationRole Role { get; set; }
}