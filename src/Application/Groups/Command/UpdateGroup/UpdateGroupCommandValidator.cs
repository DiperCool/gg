using CleanArchitecture.Application.Common.Validators;
using FluentValidation;

namespace CleanArchitecture.Application.Groups.Command.UpdateGroup;

public class UpdateGroupCommandValidator: AbstractValidator<UpdateGroupCommand>
{
    public UpdateGroupCommandValidator()
    {
        RuleFor(x => x.Group).SetValidator(new GroupModelExtendedValidator());
    }
}