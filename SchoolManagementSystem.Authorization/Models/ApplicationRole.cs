using Microsoft.AspNetCore.Identity;

namespace SchoolManagementSystem.Authorization.Models;

public class ApplicationRole : IdentityRole
{
	public ICollection<ApplicationUserRole> RoleUsers { get; set; } = new List<ApplicationUserRole>();
}