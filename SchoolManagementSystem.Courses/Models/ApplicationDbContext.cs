using Microsoft.EntityFrameworkCore;

namespace SchoolManagementSystem.Courses.Models;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public DbSet<Course> Courses { get; set; }

	public DbSet<StudentCourse> StudentCourses { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Course>().HasIndex(c => new { c.Name, c.SpecializeId }).IsUnique();

		modelBuilder.Entity<StudentCourse>().HasKey(sc => new { sc.CourseId, sc.StudentId });
	}
}