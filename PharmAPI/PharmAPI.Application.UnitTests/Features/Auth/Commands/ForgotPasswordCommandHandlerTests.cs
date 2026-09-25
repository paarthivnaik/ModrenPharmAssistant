using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PharmAPI.Application.Common.Interfaces;
using PharmAPI.Application.Features.Auth.Commands.ForgotPassword;
using PharmAPI.Domain.Entities;
using Xunit;

namespace PharmAPI.Application.UnitTests.Features.Auth.Commands;

public class ForgotPasswordCommandHandlerTests
{
    private readonly Mock<IIdentityService> _identityServiceMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<ILogger<ForgotPasswordCommandHandler>> _loggerMock;
    private readonly ForgotPasswordCommandHandler _handler;

    public ForgotPasswordCommandHandlerTests()
    {
        _identityServiceMock = new Mock<IIdentityService>();
        _emailServiceMock = new Mock<IEmailService>();
        _loggerMock = new Mock<ILogger<ForgotPasswordCommandHandler>>();

        _handler = new ForgotPasswordCommandHandler(
            _identityServiceMock.Object,
            _emailServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserExistsAndIsActive_GeneratesTokenAndSendsEmail()
    {
        // Arrange
        var email = "pharmacist@example.com";
        var user = new ApplicationUser { Id = "user-1", Email = email, UserName = email, IsActive = true };
        var token = "cryptographic-reset-token-123";

        _identityServiceMock.Setup(x => x.FindByEmailAsync(email))
            .ReturnsAsync(user);

        _identityServiceMock.Setup(x => x.GeneratePasswordResetTokenAsync(user))
            .ReturnsAsync(token);

        var command = new ForgotPasswordCommand(email);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Message.Should().Contain("If an account with that email address exists");

        _emailServiceMock.Verify(x => x.SendPasswordResetEmailAsync(
            email,
            It.Is<string>(url => url.Contains("pharmacist") && url.Contains(token)),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ReturnsGenericMessageWithoutSendingEmail()
    {
        // Arrange
        var email = "nonexistent@example.com";

        _identityServiceMock.Setup(x => x.FindByEmailAsync(email))
            .ReturnsAsync((ApplicationUser?)null);

        var command = new ForgotPasswordCommand(email);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Message.Should().Contain("If an account with that email address exists");

        _emailServiceMock.Verify(x => x.SendPasswordResetEmailAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
