using FluentValidation;

namespace CleanArchitecture.Application.AdminPanel.Query.GetLogs;

public class GetLogsQueryValidator: AbstractValidator<GetLogsQuery>
{
    public GetLogsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1);
    }
}