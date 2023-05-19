using FluentValidation;

namespace CleanArchitecture.Application.Profiles.Command.ConfirmEmail;

public class ConfirmEmailCommandValidator: AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}