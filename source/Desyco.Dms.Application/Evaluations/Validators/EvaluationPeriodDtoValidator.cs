using Desyco.Dms.Application.Evaluations.DTOs;
using FluentValidation;

namespace Desyco.Dms.Application.Evaluations.Validators;

public class EvaluationPeriodDtoValidator : AbstractValidator<EvaluationPeriodDto>
{
    public EvaluationPeriodDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

        RuleFor(x => x.ShortName)
            .NotEmpty().WithMessage("ShortName is required.")
            .MaximumLength(25).WithMessage("ShortName must not exceed 25 characters.");

        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Weight must be greater than 0.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("StartDate is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("EndDate is required.")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("EndDate must be greater than or equal to StartDate.");

        RuleFor(x => x.AcademicYearId)
            .GreaterThan(0).WithMessage("AcademicYearId is required.");

        RuleFor(x => x.LevelTypeId)
            .GreaterThan(0).WithMessage("LevelTypeId is required.");
    }
}
