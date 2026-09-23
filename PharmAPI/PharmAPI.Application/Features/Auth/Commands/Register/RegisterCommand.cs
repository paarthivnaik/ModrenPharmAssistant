using MediatR;
using PharmAPI.Application.Features.Auth.DTOs;

namespace PharmAPI.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string ConfirmPassword,
    string FullName,
    string ContactNo,
    string Address,
    string? Role = "Staff"
) : IRequest<RegisterResponseDto>;
