using System.Data;
using FluentValidation;

namespace CleanArchitecture.Application.Common.Validators;

public class StageValidator : AbstractValidator<StageModel>
{
    public StageValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleForEach(x => x.Groups).SetValidator(new GroupValidator());
    }
}