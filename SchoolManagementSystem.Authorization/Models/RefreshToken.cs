namespace SchoolManagementSystem.Authorization.Models;

public class RefreshToken
{
	public RefreshToken()
	{
	}

	public RefreshToken(string jwtId, string userId, DateTime expiryDate, string salt, string tokenHash)
	{
		JwtId = jwtId;
		UserId = userId;
		ExpiryDate = expiryDate;
		Salt = salt;
		TokenHash = tokenHash;
	}

	public string Id { get; set; } = Guid.NewGuid().ToString();

	public string UserId { get; set; }

	public string Salt { get; set; }

	public string TokenHash { get; set; }

	public string JwtId { get; set; }

	public DateTime ExpiryDate { get; set; }

	public ApplicationUser User { get; set; }
}