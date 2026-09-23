using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmAPI.Application.Features.Auth.Commands.Login;
using PharmAPI.Application.Features.Auth.Commands.Register;
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
    /// Registers a new pharmacy staff user account.
    /// </summary>
    /// <param name="command">User registration details</param>
    /// <returns>Created user summary</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterCommand command)
    {
        try
        {
            var response = await Mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, response);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = "Validation failed", errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Registration failed: " + ex.Message });
        }
    }
}
