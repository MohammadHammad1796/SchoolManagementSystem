using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Authorization.Models;

public class CustomUserStore :
	UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>
{
	public CustomUserStore(ApplicationDbContext context)
		: base(context)
	{
		AutoSaveChanges = false;
	}
}