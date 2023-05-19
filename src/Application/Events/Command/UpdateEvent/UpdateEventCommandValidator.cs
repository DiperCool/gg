using FluentValidation;

namespace CleanArchitecture.Application.Events.Command.UpdateEvent;

public class UpdateEventCommandValidator: AbstractValidator<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(x => x.EntryPrice).GreaterThanOrEqualTo(0);
    }
}