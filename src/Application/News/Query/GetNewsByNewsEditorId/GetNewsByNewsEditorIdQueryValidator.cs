using FluentValidation;

namespace CleanArchitecture.Application.News.Query.GetNewsByNewsEditorId;

public class GetNewsByNewsEditorIdQueryValidator: AbstractValidator<GetNewsByNewsEditorIdQuery>
{
    public GetNewsByNewsEditorIdQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
    }
}