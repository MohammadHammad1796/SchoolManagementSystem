using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Courses.Models;

public class Course
{
	public int Id { get; set; }

	[Required]
	[MaxLength(50)]
	public string Name { get; set; }

	public int SpecializeId { get; set; }
}