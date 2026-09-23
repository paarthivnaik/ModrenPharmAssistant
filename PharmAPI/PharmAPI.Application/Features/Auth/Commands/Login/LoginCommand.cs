using MediatR;
using PharmAPI.Application.Features.Auth.DTOs;

namespace PharmAPI.Application.Features.Auth.Commands.Login;

public record LoginCommand(
    string Email,
    string Password,
    bool RememberMe = false
) : IRequest<LoginResponseDto>;
