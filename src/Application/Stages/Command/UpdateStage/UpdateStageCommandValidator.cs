using FluentValidation;

namespace CleanArchitecture.Application.Stages.Command.UpdateStage;

public class UpdateStageCommandValidator : AbstractValidator<UpdateStageCommand>
{
    public UpdateStageCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}