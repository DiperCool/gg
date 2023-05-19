using FluentValidation;

namespace CleanArchitecture.Application.News.Query.GetNews;

public class GetNewsQueryValidator: AbstractValidator<GetNewsQuery>
{
    public GetNewsQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
    }
}