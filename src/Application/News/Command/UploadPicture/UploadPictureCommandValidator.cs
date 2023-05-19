using CleanArchitecture.Application.Common.Validators;
using FluentValidation;

namespace CleanArchitecture.Application.News.Command.UploadPicture;

public class UploadPictureCommandValidator: AbstractValidator<UploadPictureCommand>
{
    public UploadPictureCommandValidator()
    {
        RuleFor(x => x.Picture)
            .SetValidator(new ImageValidator());
    }
}