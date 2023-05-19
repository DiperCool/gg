using FluentValidation;

namespace CleanArchitecture.Application.AdminPanel.Command.RefreshTokens;

public class EmployeeRefreshTokensCommandValidator: AbstractValidator<EmployeeRefreshTokensCommand>
{
    public EmployeeRefreshTokensCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}