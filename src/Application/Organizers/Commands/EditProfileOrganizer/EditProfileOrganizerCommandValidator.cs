using FluentValidation;

namespace CleanArchitecture.Application.Organizers.Commands.EditProfileOrganizer;

public class EditProfileOrganizerCommandValidator: AbstractValidator<EditProfileOrganizerCommand>
{
    public EditProfileOrganizerCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50);
    }
}