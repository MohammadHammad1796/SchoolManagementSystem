using Microsoft.EntityFrameworkCore;

namespace SchoolManagementSystem.Admission.Models;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
	: base(options)
	{
	}

	public DbSet<Specialize> Specializes { get; set; }

	public DbSet<Student> Students { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Specialize>().HasIndex(s => s.Name).IsUnique();

		modelBuilder.Entity<Student>().HasIndex(s => s.UserId).IsUnique();
	}
}