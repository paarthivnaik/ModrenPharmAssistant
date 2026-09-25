using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PharmAPI.Application.Common.Interfaces;
using PharmAPI.Application.Features.Auth.Commands.ResetPassword;
using Xunit;

namespace PharmAPI.Application.UnitTests.Features.Auth.Commands;

public class ResetPasswordCommandHandlerTests
{
    private readonly Mock<IIdentityService> _identityServiceMock;
    private readonly Mock<ILogger<ResetPasswordCommandHandler>> _loggerMock;
    private readonly ResetPasswordCommandHandler _handler;

    public ResetPasswordCommandHandlerTests()
    {
        _identityServiceMock = new Mock<IIdentityService>();
        _loggerMock = new Mock<ILogger<ResetPasswordCommandHandler>>();

        _handler = new ResetPasswordCommandHandler(
            _identityServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenResetSucceeds_ReturnsSuccessMessage()
    {
        // Arrange
        var email = "pharmacist@example.com";
        var token = "valid-token";
        var newPassword = "NewSecurePassword123!";

        _identityServiceMock.Setup(x => x.ResetPasswordAsync(email, token, newPassword, It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, null));

        var command = new ResetPasswordCommand(email, token, newPassword, newPassword);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Message.Should().Contain("Password has been reset successfully");
    }

    [Fact]
    public async Task Handle_WhenResetFails_ThrowsInvalidOperationException()
    {
        // Arrange
        var email = "pharmacist@example.com";
        var token = "expired-token";
        var newPassword = "NewSecurePassword123!";

        _identityServiceMock.Setup(x => x.ResetPasswordAsync(email, token, newPassword, It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, "The token is invalid or expired."));

        var command = new ResetPasswordCommand(email, token, newPassword, newPassword);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}
