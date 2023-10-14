using Microsoft.AspNetCore.Identity;

namespace SchoolManagementSystem.Authorization.Models;

public class ApplicationUserRole : IdentityUserRole<string>
{
	public ApplicationUser User { get; set; }

	public ApplicationRole Role { get; set; }
}