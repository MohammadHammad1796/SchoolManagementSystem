namespace SchoolManagementSystem.Shared.Auth;

public static class Roles
{
	public const string Admin = "Admin";

	public const string Student = "Student";

	public const string NoRole = "NoRole";

	public const string NoRoleOrStudent = $"{NoRole},{Student}";
}