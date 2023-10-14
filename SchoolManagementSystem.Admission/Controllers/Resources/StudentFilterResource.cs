namespace SchoolManagementSystem.Admission.Controllers.Resources;

public class StudentFilterResource
{
	public int? SpecializeId { get; set; }

	public string Status { get; set; }

	public int[] StudentIds { get; set; } = Array.Empty<int>();

	public bool OnlyRegisterdOnCourses { get; set; }
}