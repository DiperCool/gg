using CleanArchitecture.Application.Common.Validators;
using FluentValidation;

namespace CleanArchitecture.Application.Profiles.Command.UpdateProfilePicture;

public class UpdateProfilePictureCommandValidator: AbstractValidator<UpdateProfilePictureCommand>
{
    private readonly List<string> allowedExtensions = new()
    {
        "jpg", "JPG", "png", "PNG", "jpeg", "JPEG",
    };
    public UpdateProfilePictureCommandValidator()
    {
        RuleFor(x => x.File)
            .SetValidator(new ImageValidator()!);
    }

    private bool HaveAllowedExtension(string str)
    {
        var extension = str.Split('.').Last();
        return allowedExtensions.Contains(extension);
    }
}