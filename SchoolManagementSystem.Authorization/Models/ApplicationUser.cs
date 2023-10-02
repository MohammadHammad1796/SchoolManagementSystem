using Microsoft.AspNetCore.Identity;

namespace SchoolManagementSystem.Authorization.Models;

public class ApplicationUser : IdentityUser
{
	public ICollection<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();

	public ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
}