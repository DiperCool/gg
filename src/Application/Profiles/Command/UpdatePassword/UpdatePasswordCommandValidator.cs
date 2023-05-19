using FluentValidation;

namespace CleanArchitecture.Application.Profiles.Command.UpdatePassword;

public class UpdatePasswordCommandValidator: AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty();
        RuleFor(x=>x.NewPassword)
            .MinimumLength(6)
            .MaximumLength(50)
            .Equal(x=>x.ConfirmNewPassword).WithMessage("The confirmed password does not match the password")
            .NotEmpty();
    }
}