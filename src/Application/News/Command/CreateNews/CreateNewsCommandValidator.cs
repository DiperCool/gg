using CleanArchitecture.Application.Common.Validators;
using FluentValidation;

namespace CleanArchitecture.Application.News.Command.CreateNews;

public class CreateNewsCommandValidator : AbstractValidator<CreateNewsCommand>
{
    public CreateNewsCommandValidator()
    {
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Background).SetValidator(new ImageValidator()!);
    }
}