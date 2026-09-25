using MediatR;
using Microsoft.Extensions.Logging;
using PharmAPI.Application.Common.Interfaces;
using PharmAPI.Application.Features.Auth.DTOs;

namespace PharmAPI.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordResponseDto>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<ResetPasswordCommandHandler> _logger;

    public ResetPasswordCommandHandler(
        IIdentityService identityService,
        ILogger<ResetPasswordCommandHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<ResetPasswordResponseDto> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var (success, errorMessage) = await _identityService.ResetPasswordAsync(
            request.Email,
            request.Token,
            request.NewPassword,
            cancellationToken);

        if (!success)
        {
            _logger.LogWarning("Password reset failed for {Email}: {Error}", request.Email, errorMessage);
            throw new InvalidOperationException(errorMessage ?? "Password reset failed. The reset token may be invalid or expired.");
        }

        _logger.LogInformation("Password reset successful for {Email}. Prior sessions revoked.", request.Email);
        return new ResetPasswordResponseDto("Password has been reset successfully. Please log in with your new credentials.");
    }
}
