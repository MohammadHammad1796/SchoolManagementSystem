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

			b.HasMany(au => au.Roles)
				.WithMany(ar => ar.Users)
				.UsingEntity<ApplicationUserRole>(builder =>
						builder
							.HasOne(userRole => userRole.Role)
							.WithMany(role => role.RoleUsers)
							.HasForeignKey(x => x.RoleId)
							.IsRequired(),
					builder => builder
						.HasOne(userRole => userRole.User)
						.WithMany(user => user.UserRoles)
						.HasForeignKey(x => x.UserId)
						.IsRequired());
		});

		modelBuilder.Entity<ApplicationRole>(b =>
		{
			b.HasMany(ar => ar.RoleUsers)
				.WithOne(ur => ur.Role)
				.HasForeignKey(ur => ur.RoleId)
				.IsRequired();

			b.HasMany(ar => ar.Users)
				.WithMany(au => au.Roles)
				.UsingEntity<ApplicationUserRole>()
				.HasKey(aur => aur.RoleId);
		});

		modelBuilder.Entity<ApplicationUserRole>().ToTable("AspNetUserRoles");
		modelBuilder.Entity<ApplicationUserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });

		modelBuilder.Entity<RefreshToken>().HasIndex(rt => rt.JwtId).IsUnique();
		modelBuilder.Entity<RefreshToken>().HasIndex(rt => rt.TokenHash).IsUnique();
		modelBuilder.Entity<RefreshToken>().HasIndex(rt => rt.Salt).IsUnique();
	}
}