using FluentValidation;

namespace CleanArchitecture.Application.Profiles.Command.EditProfile;

public class EditProfileCommandValidator: AbstractValidator<EditProfileCommand>
{
    public EditProfileCommandValidator()
    {
        When(x => !string.IsNullOrEmpty(x.Discord), () =>
        {
            RuleFor(x => x.Discord).MaximumLength(50);
        });
        When(x => !string.IsNullOrEmpty(x.Email), () =>
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Email).MaximumLength(50);
        });
        
        When(x => !string.IsNullOrEmpty(x.Name), () =>
        {
            RuleFor(x => x.Name).MaximumLength(50);
        });
        When(x => !string.IsNullOrEmpty(x.Nickname), () =>
        {
            RuleFor(x => x.Nickname).MaximumLength(50);
        });
        When(x => !string.IsNullOrEmpty(x.Telegram), () =>
        {
            RuleFor(x => x.Telegram).MaximumLength(50);
        });
        When(x => !string.IsNullOrEmpty(x.Youtube), () =>
        {
            RuleFor(x => x.Youtube).MaximumLength(50);
        });
        When(x => !string.IsNullOrEmpty(x.PubgId), () =>
        {
            RuleFor(x => x.PubgId).MaximumLength(50);
        });

    }
}