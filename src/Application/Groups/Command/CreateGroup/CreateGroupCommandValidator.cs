using CleanArchitecture.Application.Common.Validators;
using FluentValidation;

namespace CleanArchitecture.Application.Groups.Command.CreateGroup;

public class CreateGroupCommandValidator: AbstractValidator<CreateGroupCommand>
{
    public CreateGroupCommandValidator()
    {
        RuleFor(x => x.GroupModel).SetValidator(new GroupValidator());
    }
}