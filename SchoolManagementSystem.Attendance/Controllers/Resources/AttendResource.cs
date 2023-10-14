namespace SchoolManagementSystem.Attendance.Controllers.Resources;

public class AttendResource
{
	public bool IsAttended { get; set; }

	public DateTime Date { get; set; }

	public int StudentId { get; set; }
}