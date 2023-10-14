using SchoolManagementSystem.Shared.Controllers.Resources;

namespace SchoolManagementSystem.Courses.Controllers.Resources;

public class CourseOptionsResource
{
	public CourseFilterResource Filters { get; set; }

	public SortResource Sort { get; set; }

	public PaginateResource Paginate { get; set; } = new PaginateResource();
}