using FluentValidation;

namespace CleanArchitecture.Application.AdminPanel.Query.GetLogsByEmployeeId;

public class GetLogsByEmployeeIdQueryValidator : AbstractValidator<GetLogsByEmployeeIdQuery>
{
    public GetLogsByEmployeeIdQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1);
    }
}