namespace PharmAPI.Application.Features.Auth.DTOs;

public record RegisterResponseDto(
    string UserId,
    string Email,
    string FullName,
    List<string> Roles,
    string Message
);
