using Authorization.Controllers.Resources;
using Authorization.Models;

namespace Authorization.Services;

public interface IAccountsService
{
	bool IsAccountLocked(ApplicationUser user);

	Task AccessFailedAsync(ApplicationUser user);

	Task<JwtResource> GenerateJwtAsync(ApplicationUser user);

	Task<Result<RefreshToken>> GetRefreshTokenIfValidAsync(string accessToken, string refreshToken);
}