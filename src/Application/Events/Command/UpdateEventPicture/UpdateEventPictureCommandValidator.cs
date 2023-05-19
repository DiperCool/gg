using CleanArchitecture.Application.Common.Validators;
using FluentValidation;

namespace CleanArchitecture.Application.Events.Command.UpdateEventPicture;

public class UpdateEventPictureCommandValidator: AbstractValidator<UpdateEventPictureCommand>
{
    public UpdateEventPictureCommandValidator()
    {
        RuleFor(x => x.File).SetValidator(new ImageValidator()!);
    }
}