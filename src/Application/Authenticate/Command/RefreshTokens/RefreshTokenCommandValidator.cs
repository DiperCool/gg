using FluentValidation;

namespace CleanArchitecture.Application.Authenticate.Command.RefreshTokens;

public class RefreshTokenCommandValidator: AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}