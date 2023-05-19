using CleanArchitecture.Domain.Entities.Events;
using FluentValidation;

namespace CleanArchitecture.Application.Common.Validators;

public class GroupValidator : AbstractValidator<GroupModel>
{
    public GroupValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.SlotPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.SlotsQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ReserveSlotsQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PaidSlots).GreaterThanOrEqualTo(0);


    }
}