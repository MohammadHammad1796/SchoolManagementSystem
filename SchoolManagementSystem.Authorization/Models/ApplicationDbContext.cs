using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagementSystem.Authorization.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
	IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
	IdentityRoleClaim<string>, IdentityUserToken<string>>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<ApplicationUser>(b =>
		{
			b.HasMany(au => au.UserRoles)
				.WithOne(ur => ur.User)
				.HasForeignKey(ur => ur.UserId)
				.IsRequired();
		});

		modelBuilder.Entity<ApplicationRole>(b =>
		{
			b.HasMany(ar => ar.RoleUsers)
				.WithOne(ur => ur.Role)
				.HasForeignKey(ur => ur.RoleId)
				.IsRequired();
		});

		modelBuilder.Entity<ApplicationUserRole>().ToTable("AspNetUserRoles");
		modelBuilder.Entity<ApplicationUserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });

		modelBuilder.Entity<RefreshToken>().HasIndex(rt => rt.JwtId).IsUnique();
		modelBuilder.Entity<RefreshToken>().HasIndex(rt => rt.TokenHash).IsUnique();
		modelBuilder.Entity<RefreshToken>().HasIndex(rt => rt.Salt).IsUnique();
	}
}