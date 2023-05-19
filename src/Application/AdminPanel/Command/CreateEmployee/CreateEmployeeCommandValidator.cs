using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.AdminPanel.Command.CreateEmployee;

public class CreateEmployeeCommandValidator: AbstractValidator<CreateEmployeeCommand>
{
    private IApplicationDbContext _application;
    public CreateEmployeeCommandValidator(IApplicationDbContext application)
    {
        _application = application;
        RuleFor(x=>x.Password)
            .MinimumLength(6)
            .MaximumLength(50)
            .Equal(x=>x.ConfirmPassword).WithMessage("The confirmed password does not match the password")
            .NotEmpty();
        RuleFor(x => x.Nickname)
            .MinimumLength(6)
            .MaximumLength(50)
            .NotEmpty()
            .MustAsync(IsNicknameExist).WithMessage("{PropertyName} is already exists");
        RuleFor(x => x.Nickname)
            .MaximumLength(50)
            .NotEmpty();
        RuleFor(x => x.Role)
            .MustAsync(IsRoleExist).WithMessage("Role doesn't exists")
            .NotEmpty();
    }
    private async Task<bool> IsNicknameExist(string nickname, CancellationToken cancellationToken)
    {
        return !await _application.Employees.AnyAsync(x => x.Nickname== nickname, cancellationToken: cancellationToken);
    }

    private async Task<bool> IsRoleExist(string role, CancellationToken cancellationToken)
    {
        return await _application.EmployeeRoles.AnyAsync(x => x.Role==role, cancellationToken: cancellationToken);

    }
}