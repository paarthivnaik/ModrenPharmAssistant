using MediatR;
using PharmAPI.Application.Common.Interfaces;
using PharmAPI.Application.Features.Auth.DTOs;

namespace PharmAPI.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(IIdentityService identityService, IJwtTokenService jwtTokenService)
    {
        _identityService = identityService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var (success, isLockedOut, errorMessage, user, roles) = await _identityService.AuthenticateAsync(
            request.Email,
            request.Password,
            request.RememberMe,
            cancellationToken);

        if (isLockedOut)
        {
            throw new UnauthorizedAccessException("Account is locked due to repeated failed login attempts. Please try again after 15 minutes.");
        }

        if (!success || user == null)
        {
            throw new UnauthorizedAccessException(errorMessage ?? "Invalid email or password.");
        }

        var rolesList = roles?.ToList() ?? new List<string>();
        var token = _jwtTokenService.GenerateToken(user, rolesList, out var expiration);

        // Role-based routing: Staff -> /sales, Admin -> /dashboard
        var defaultRedirectUrl = rolesList.Contains("Staff") ? "/sales" : "/dashboard";

        return new LoginResponseDto(
            Token: token,
            Expiration: expiration,
            Email: user.Email ?? request.Email,
            FullName: user.FullName ?? user.UserName ?? "User",
            Roles: rolesList,
            DefaultRedirectUrl: defaultRedirectUrl
        );
    }
}
