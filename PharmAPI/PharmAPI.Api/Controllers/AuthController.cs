using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmAPI.Application.Features.Auth.Commands.ForgotPassword;
using PharmAPI.Application.Features.Auth.Commands.Login;
using PharmAPI.Application.Features.Auth.Commands.ResetPassword;
using PharmAPI.Application.Features.Auth.DTOs;

namespace PharmAPI.Api.Controllers;

[AllowAnonymous]
public class AuthController : ApiControllerBase
{
    /// <summary>
    /// Authenticates a pharmacy user and generates a JWT Bearer token.
    /// </summary>
    /// <param name="command">User login credentials</param>
    /// <returns>JWT authentication token and user profile summary</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status423Locked)]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginCommand command)
    {
        try
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = "Validation failed", errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (UnauthorizedAccessException ex)
        {
            if (ex.Message.Contains("locked", StringComparison.OrdinalIgnoreCase))
            {
                return StatusCode(StatusCodes.Status423Locked, new { message = ex.Message });
            }
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception)
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }
    }

    /// <summary>
    /// Initiates self-service password recovery by dispatching a time-limited reset link.
    /// </summary>
    /// <param name="command">User registered email address</param>
    /// <returns>Generic operation confirmation to prevent account enumeration</returns>
    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(ForgotPasswordResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ForgotPasswordResponseDto>> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        try
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = "Validation failed", errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Resets the user's password using the cryptographic reset token and invalidates active sessions.
    /// </summary>
    /// <param name="command">Reset token, email, and new password</param>
    /// <returns>Password reset confirmation</returns>
    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(ResetPasswordResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ResetPasswordResponseDto>> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        try
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = "Validation failed", errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

