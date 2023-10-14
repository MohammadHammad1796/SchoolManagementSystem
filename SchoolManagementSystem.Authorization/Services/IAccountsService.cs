using SchoolManagementSystem.Authorization.Controllers.Resources;
using SchoolManagementSystem.Authorization.Models;
using SchoolManagementSystem.Shared.Models;

namespace SchoolManagementSystem.Authorization.Services;

public interface IAccountsService
{
	bool IsAccountLocked(ApplicationUser user);

	Task AccessFailedAsync(ApplicationUser user);

	Task<JwtResource> GenerateJwtAsync(ApplicationUser user);

	Task<Result<RefreshToken>> GetRefreshTokenIfValidAsync(string accessToken, string refreshToken);

	Task RemoveFromRoleAsync(ApplicationUser user, string roleName);
}