using CleanArchitecture.Application.Common.Validators;
using FluentValidation;

namespace CleanArchitecture.Application.News.Command.UpdateNewsPicture;

public class UpdateNewsPictureCommandValidator: AbstractValidator<UpdateBackgroundCommand>
{
    public UpdateNewsPictureCommandValidator()
    {
        RuleFor(x => x.File)
            .SetValidator(new ImageValidator()!);
    }
}