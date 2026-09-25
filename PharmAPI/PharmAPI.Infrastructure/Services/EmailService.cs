using Microsoft.Extensions.Logging;
using PharmAPI.Application.Common.Interfaces;

namespace PharmAPI.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendPasswordResetEmailAsync(string toEmail, string resetLink, CancellationToken cancellationToken = default)
    {
        // Pluggable email delivery service (logs to console/structured logs and ready for SMTP/SendGrid client)
        _logger.LogInformation(
            "=== [OUTGOING EMAIL: PASSWORD RESET] ===\nTo: {ToEmail}\nSubject: Reset Your Password - Modern PharmAssistant\nReset Link: {ResetLink}\n========================================",
            toEmail,
            resetLink);

        return Task.CompletedTask;
    }
}
