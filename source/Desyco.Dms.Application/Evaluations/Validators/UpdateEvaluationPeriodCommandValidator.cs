using Desyco.Dms.Application.Evaluations.Commands;
using FluentValidation;

namespace Desyco.Dms.Application.Evaluations.Validators;

public class UpdateEvaluationPeriodCommandValidator : AbstractValidator<UpdateEvaluationPeriodCommand>
{
    public UpdateEvaluationPeriodCommandValidator()
    {
        RuleFor(x => x.EvaluationPeriod.Id)
            .GreaterThan(0).WithMessage("Id is required.");

        RuleFor(x => x.EvaluationPeriod).SetValidator(new EvaluationPeriodDtoValidator());
    }
}
