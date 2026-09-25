using MediatR;
using PharmAPI.Application.Features.Auth.DTOs;

namespace PharmAPI.Application.Features.Auth.Commands.ForgotPassword;

public record ForgotPasswordCommand(
    string Email
) : IRequest<ForgotPasswordResponseDto>;
