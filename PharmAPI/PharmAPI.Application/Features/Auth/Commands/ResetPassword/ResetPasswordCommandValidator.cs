using FluentValidation;

namespace PharmAPI.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(v => v.Token)
            .NotEmpty().WithMessage("Reset token is required.");

        RuleFor(v => v.NewPassword)
            .NotEmpty().WithMessage("New password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        RuleFor(v => v.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm password is required.")
            .Equal(v => v.NewPassword).WithMessage("Passwords do not match.");
    }
}
