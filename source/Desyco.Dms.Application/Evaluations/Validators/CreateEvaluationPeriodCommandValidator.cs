using Desyco.Dms.Application.Evaluations.Commands;
using FluentValidation;

namespace Desyco.Dms.Application.Evaluations.Validators;

public class CreateEvaluationPeriodCommandValidator : AbstractValidator<CreateEvaluationPeriodCommand>
{
    public CreateEvaluationPeriodCommandValidator()
    {
        RuleFor(x => x.EvaluationPeriod).SetValidator(new EvaluationPeriodDtoValidator());
    }
}
