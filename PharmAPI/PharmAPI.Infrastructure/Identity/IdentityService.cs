using Microsoft.AspNetCore.Identity;
using PharmAPI.Application.Common.Interfaces;
using PharmAPI.Domain.Entities;

namespace PharmAPI.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<(bool Success, bool IsLockedOut, string? ErrorMessage, ApplicationUser? User, IList<string>? Roles)> AuthenticateAsync(
        string email,
        string password,
        bool rememberMe,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || !user.IsActive)
        {
            // Do not leak whether the email exists
            return (false, false, "Invalid email or password.", null, null);
        }

        // Check if user is already locked out
        if (await _userManager.IsLockedOutAsync(user))
        {
            return (false, true, "Account is locked due to repeated failed login attempts. Please try again after 15 minutes.", user, null);
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            return (false, true, "Account is locked due to repeated failed login attempts. Please try again after 15 minutes.", user, null);
        }

        if (!result.Succeeded)
        {
            return (false, false, "Invalid email or password.", null, null);
        }

        // Reset access failed count upon successful login
        await _userManager.ResetAccessFailedCountAsync(user);

        var roles = await _userManager.GetRolesAsync(user);

        return (true, false, null, user, roles);
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }
}
