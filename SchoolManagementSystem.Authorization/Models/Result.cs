namespace SchoolManagementSystem.Authorization.Models;

public class Result<T>
{
	public bool Succeeded { get; set; }

	public T ObjectResult { get; set; }
}