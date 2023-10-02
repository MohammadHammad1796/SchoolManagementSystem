namespace Authorization.Controllers.Resources;

public class JwtResource
{
	public JwtResource(
		string accessToken,
		string refreshToken,
		DateTime refreshExpireAt)
	{
		AccessToken = accessToken;
		RefreshToken = refreshToken;
		RefreshExpireAt = refreshExpireAt;
	}

	public string AccessToken { get; set; }

	public string RefreshToken { get; set; }

	public DateTime RefreshExpireAt { get; set; }
}