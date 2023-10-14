namespace SchoolManagementSystem.Shared.Models;

public class Result<T>
{
	public bool Succeeded { get; set; }

	public T ObjectResult { get; set; }

	public Result(bool succeeded = false, T objectResult = default)
	{
		Succeeded = succeeded;
		ObjectResult = objectResult;
	}

	public Result()
	{
	}
}