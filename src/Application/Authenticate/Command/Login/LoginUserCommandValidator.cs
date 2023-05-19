using FluentValidation;

namespace CleanArchitecture.Application.Authenticate.Command.Login
{
    public class LoginUserCommandValidator: AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x=>x.Email)
                .NotEmpty();
            RuleFor(x=>x.Password)
                .NotEmpty();
        }
    }
}