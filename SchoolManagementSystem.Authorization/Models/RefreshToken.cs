using System.ComponentModel.DataAnnotations;

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

	[Required]
	public string Id { get; set; } = Guid.NewGuid().ToString();

	[Required]
	public string UserId { get; set; }

	[Required]
	public string Salt { get; set; }

	[Required]
	public string TokenHash { get; set; }

	[Required]
	public string JwtId { get; set; }

	public DateTime ExpiryDate { get; set; }

	public ApplicationUser User { get; set; }
}