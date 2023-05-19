using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Authenticate.Command.GenerateRestorePassword;

public class GenerateRestorePasswordCommandValidator: AbstractValidator<GenerateRestorePasswordCommand>
{
    public GenerateRestorePasswordCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .MustAsync((async (email, token) => await context.Users.AnyAsync(x => x.Profile.Email == email)))
            .WithMessage("User with this email doesn't exist");
    }
}