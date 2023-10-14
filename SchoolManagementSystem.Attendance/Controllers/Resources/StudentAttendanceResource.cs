namespace SchoolManagementSystem.Attendance.Controllers.Resources;

public class StudentAttendanceResource
{
	public int Id { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string MotherFullName { get; set; }

	public string FatherFirstName { get; set; }

	public bool? IsAttended { get; set; }
}