namespace SchoolManagementSystem.Attendance.Models;

public class StudentAttendance
{
	public int Id { get; set; }

	public int StudentId { get; set; }

	public int AttendanceDateId { get; set; }

	public AttendanceDate AttendanceDate { get; set; }

	public bool IsAttended { get; set; }
}