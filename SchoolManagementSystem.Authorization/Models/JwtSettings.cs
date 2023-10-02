namespace SchoolManagementSystem.Authorization.Models;

public class JwtSettings
{
	public string Secret { get; set; }

	public string Audience { get; set; }

	public string Issuer { get; set; }

	public int LifeTime { get; set; }

	public int RefreshLifeTime { get; set; }
}