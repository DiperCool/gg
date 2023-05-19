using CleanArchitecture.Application.Common.Validators;
using FluentValidation;

namespace CleanArchitecture.Application.Stages.Command.CreateStage;

public class CreateStageCommandValidator: AbstractValidator<CreateStageCommand>
{
    public CreateStageCommandValidator()
    {
        RuleFor(x => x.Stage).SetValidator(new StageWithWinnersModelValidator());
    }
}