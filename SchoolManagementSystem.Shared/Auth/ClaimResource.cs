namespace SchoolManagementSystem.Shared.Auth;

public class ClaimResource
{
	public string Type { get; set; }
	public string Value { get; set; }

	public ClaimResource(string type, string value)
	{
		Type = type;
		Value = value;
	}
}