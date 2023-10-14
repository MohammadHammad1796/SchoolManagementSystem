using SchoolManagementSystem.Shared.Controllers.Resources;

namespace SchoolManagementSystem.Admission.Controllers.Resources;

public class StudentOptionsResource
{
	public StudentFilterResource Filters { get; set; }

	public SortResource Sort { get; set; }

	public PaginateResource Paginate { get; set; } = new PaginateResource();
}