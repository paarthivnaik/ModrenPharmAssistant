using MediatR;
using PharmAPI.Application.Common.Interfaces;
using PharmAPI.Application.Features.Auth.DTOs;

namespace PharmAPI.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponseDto>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<RegisterResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var (success, errorMessage, user, roles) = await _identityService.RegisterUserAsync(
            email: request.Email,
            password: request.Password,
            fullName: request.FullName,
            contactNo: request.ContactNo,
            address: request.Address,
            role: string.IsNullOrWhiteSpace(request.Role) ? "Staff" : request.Role,
            cancellationToken: cancellationToken);

        if (!success || user == null)
        {
            throw new InvalidOperationException(errorMessage ?? "User registration failed.");
        }

        var rolesList = roles?.ToList() ?? new List<string> { string.IsNullOrWhiteSpace(request.Role) ? "Staff" : request.Role };

        return new RegisterResponseDto(
            UserId: user.Id,
            Email: user.Email ?? request.Email,
            FullName: user.FullName ?? request.FullName,
            Roles: rolesList,
            Message: "User account provisioned successfully."
        );
    }
}
