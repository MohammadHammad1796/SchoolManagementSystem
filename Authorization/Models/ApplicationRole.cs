﻿using Microsoft.AspNetCore.Identity;

namespace Authorization.Models;

public class ApplicationRole : IdentityRole
{
	public ICollection<ApplicationUserRole> RoleUsers { get; set; } = new List<ApplicationUserRole>();

	public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
}