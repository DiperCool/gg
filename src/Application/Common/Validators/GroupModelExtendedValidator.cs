using FluentValidation;

namespace CleanArchitecture.Application.Common.Validators;

public class GroupModelExtendedValidator : AbstractValidator<GroupModelExtended>
{
    public GroupModelExtendedValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.SlotPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.SlotsQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ReserveSlotsQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PaidSlots).GreaterThanOrEqualTo(0);

        RuleForEach(x => x.Results).SetValidator(new PlayerStatsModelValidator());
    }
}