using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Authenticate.Command.RestorePassword;

public class RestorePasswordCommandValidator: AbstractValidator<RestorePasswordCommand>
{
    private readonly IApplicationDbContext _context;
    public RestorePasswordCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x=>x.Code)
            .MustAsync(IsRestorePasswordExists).WithMessage("Code doesn't exist")
            .NotEmpty();
        RuleFor(x=>x.Password)
            .MinimumLength(6)
            .Equal(x=>x.ConfirmPassword).WithMessage("The confirmed password does not match the password")
            .NotEmpty();
    }
    private async Task<bool> IsRestorePasswordExists(Guid code, CancellationToken cancellationToken)
    {
        return await _context.RestorePasswords.AnyAsync(x => x.Id == code, cancellationToken: cancellationToken);
    }
}