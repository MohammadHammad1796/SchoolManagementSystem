using SchoolManagementSystem.Shared.Controllers.Resources;

namespace SchoolManagementSystem.Attendance.Controllers.Resources;

public class AttendanceOptionsResource
{
	public AttendanceFilterResource Filters { get; set; }

	public SortResource Sort { get; set; }

	public PaginateResource Paginate { get; set; } = new PaginateResource();
}