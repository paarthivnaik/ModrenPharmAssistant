using MediatR;
using PharmAPI.Application.Features.Auth.DTOs;

namespace PharmAPI.Application.Features.Auth.Commands.ResetPassword;

public record ResetPasswordCommand(
    string Email,
    string Token,
    string NewPassword,
    string ConfirmPassword
) : IRequest<ResetPasswordResponseDto>;
