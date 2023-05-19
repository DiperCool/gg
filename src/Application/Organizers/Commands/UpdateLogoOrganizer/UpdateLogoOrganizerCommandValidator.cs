using CleanArchitecture.Application.Common.Validators;
using FluentValidation;

namespace CleanArchitecture.Application.Organizers.Commands.UpdateLogoOrganizer;

public class UpdateLogoOrganizerCommandValidator: AbstractValidator<UpdateLogoOrganizerCommand>
{
    public UpdateLogoOrganizerCommandValidator()
    {
        RuleFor(x => x.File)
            .SetValidator(new ImageValidator()!);
    }
}