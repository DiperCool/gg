using FluentValidation;

namespace CleanArchitecture.Application.Common.Validators;

public class ImageValidator : AbstractValidator<FileModel>
{
    private readonly List<string> allowedExtensions = new()
    {
        "jpg", "JPG", "png", "PNG", "jpeg", "JPEG",
    };
    public ImageValidator()
    {
        When(x => x != null, () => {
            RuleFor(x => x.Length).LessThan(2 * 1024 * 1024);
            RuleFor(x => x.NameFile)
                .NotEmpty()
                .Must(HaveAllowedExtension)
                .WithMessage(
                    $"File extension is not allowed. Allowed extensions: {string.Join(", ", allowedExtensions)}.")
                .Length(1, 50);
        });
    }

    private bool HaveAllowedExtension(string str)
    {
        var extension = str.Split('.').Last();
        return allowedExtensions.Contains(extension);
    }
}