using FluentValidation;

namespace CleanArchitecture.Application.News.Command.EditNews;

public class EditNewsCommandValidator : AbstractValidator<EditNewsCommand>
{
    public EditNewsCommandValidator()
    {
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
    }
}