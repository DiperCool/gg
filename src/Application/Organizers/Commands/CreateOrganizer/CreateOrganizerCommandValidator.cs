using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Organizers.Commands.CreateOrganizer;

public class CreateOrganizerCommandValidator: AbstractValidator<CreateOrganizerCommand>
{
    private IApplicationDbContext _application;
    public CreateOrganizerCommandValidator(IApplicationDbContext application)
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
        RuleFor(x => x.Name)
            .MaximumLength(50)
            .NotEmpty();
    }
    private async Task<bool> IsNicknameExist(string nickname, CancellationToken cancellationToken)
    {
        return !await _application.Employees.AnyAsync(x => x.Nickname== nickname, cancellationToken: cancellationToken);
    }
}