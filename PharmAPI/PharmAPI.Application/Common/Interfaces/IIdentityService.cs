using PharmAPI.Domain.Entities;

namespace PharmAPI.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<(bool Success, bool IsLockedOut, string? ErrorMessage, ApplicationUser? User, IList<string>? Roles)> AuthenticateAsync(
        string email,
        string password,
        bool rememberMe,
        CancellationToken cancellationToken = default);

    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task<IList<string>> GetRolesAsync(ApplicationUser user);
}
