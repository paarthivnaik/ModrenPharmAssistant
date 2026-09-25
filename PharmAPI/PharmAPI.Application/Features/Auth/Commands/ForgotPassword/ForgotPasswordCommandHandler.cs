using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using PharmAPI.Application.Common.Interfaces;
using PharmAPI.Application.Features.Auth.DTOs;

namespace PharmAPI.Application.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ForgotPasswordResponseDto>
{
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;

    public ForgotPasswordCommandHandler(
        IIdentityService identityService,
        IEmailService emailService,
        ILogger<ForgotPasswordCommandHandler> logger)
    {
        _identityService = identityService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<ForgotPasswordResponseDto> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var genericMessage = "If an account with that email address exists, a password reset link has been dispatched.";

        var user = await _identityService.FindByEmailAsync(request.Email);
        if (user == null || !user.IsActive)
        {
            _logger.LogInformation("Password reset requested for non-existent or inactive email: {Email}", request.Email);
            return new ForgotPasswordResponseDto(genericMessage);
        }

        var token = await _identityService.GeneratePasswordResetTokenAsync(user);
        if (string.IsNullOrEmpty(token))
        {
            _logger.LogWarning("Failed to generate password reset token for user: {Email}", request.Email);
            return new ForgotPasswordResponseDto(genericMessage);
        }

        var resetUrl = $"http://localhost:4200/reset-password?email={WebUtility.UrlEncode(user.Email)}&token={WebUtility.UrlEncode(token)}";
        await _emailService.SendPasswordResetEmailAsync(user.Email!, resetUrl, cancellationToken);

        _logger.LogInformation("Password reset link dispatched for email: {Email}", request.Email);
        return new ForgotPasswordResponseDto(genericMessage);
    }
}
