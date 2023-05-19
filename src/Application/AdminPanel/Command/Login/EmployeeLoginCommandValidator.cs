using FluentValidation;

namespace CleanArchitecture.Application.AdminPanel.Command.Login;

public class EmployeeLoginCommandValidator: AbstractValidator<EmployeeLoginCommand>
{
    public EmployeeLoginCommandValidator()
    {
        RuleFor(x => x.Nickname).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}