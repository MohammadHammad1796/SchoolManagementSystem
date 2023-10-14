using Microsoft.EntityFrameworkCore;

namespace SchoolManagementSystem.Attendance.Models;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public DbSet<AttendanceDate> AttendanceDates { get; set; }

	public DbSet<StudentAttendance> StudentAttendances { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<AttendanceDate>().HasIndex(ad => ad.Date).IsUnique();

		modelBuilder.Entity<StudentAttendance>().HasIndex(sa => new { sa.AttendanceDateId, sa.StudentId }).IsUnique();
	}
}