namespace PharmAPI.Application.Features.Auth.DTOs;

public record LoginResponseDto(
    string Token,
    DateTime Expiration,
    string Email,
    string FullName,
    List<string> Roles,
    string DefaultRedirectUrl
);
