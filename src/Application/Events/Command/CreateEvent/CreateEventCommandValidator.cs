using CleanArchitecture.Application.Common.Validators;
using FluentValidation;

namespace CleanArchitecture.Application.Events.Command.CreateEvent;

public class CreateEventCommandValidator: AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.EntryPrice).GreaterThanOrEqualTo(0);
        RuleForEach(x => x.Stages).SetValidator(new StageValidator());
    }
}