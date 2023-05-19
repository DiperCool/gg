using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Authenticate.Command.Register
{
    public class RegisterUserCommandValidator: AbstractValidator<RegisterUserCommand>
    {
        private readonly IApplicationDbContext _context;
        public RegisterUserCommandValidator(IApplicationDbContext context)
        {
            _context = context;
            RuleFor(x=>x.Email)
                .EmailAddress()
                .MaximumLength(50)
                .MustAsync(IsEmailExist).WithMessage("{PropertyName} is already exists")
                .NotEmpty();
            RuleFor(x=>x.Password)
                .MinimumLength(6)
                .MaximumLength(50)
                .Equal(x=>x.ConfirmPassword).WithMessage("The confirmed password does not match the password")
                .NotEmpty();
            RuleFor(x => x.Name)
                .MaximumLength(50)
                .NotEmpty();
            RuleFor(x => x.Nickname)
                .MaximumLength(50)
                .NotEmpty();
            RuleFor(x => x.PubgId)
                .MaximumLength(50)
                .NotEmpty();
        }
        private async Task<bool> IsEmailExist(string email, CancellationToken cancellationToken)
        {
            return !await _context.Users.AnyAsync(x => x.Profile.Email == email, cancellationToken: cancellationToken);
        }
    }
}