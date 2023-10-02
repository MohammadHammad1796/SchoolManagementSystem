namespace Authorization.Controllers.Resources;

public class ErrorResource
{
	public string Message { get; }

	public ErrorResource(string message) => Message = message;
}

public class UnauthorizedResource : ErrorResource
{
	public UnauthorizedResource(string message, DateTimeOffset? lockoutEnd = null) : base(message)
	{
		LockoutEnd = lockoutEnd;
	}

	public DateTimeOffset? LockoutEnd { get; set; }
}