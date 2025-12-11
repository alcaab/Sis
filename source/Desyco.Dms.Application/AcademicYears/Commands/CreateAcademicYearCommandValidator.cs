using FluentValidation;

namespace Desyco.Dms.Application.AcademicYears.Commands;

public class CreateAcademicYearCommandValidator : AbstractValidator<CreateAcademicYearCommand>
{
    public CreateAcademicYearCommandValidator()
    {
        RuleFor(x => x.AcademicYear.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.AcademicYear.StartDate)
            .NotNull().WithMessage("Start date is required.");

        RuleFor(x => x.AcademicYear.EndDate)
            .NotNull().WithMessage("End date is required.")
            .GreaterThan(x => x.AcademicYear.StartDate).WithMessage("End date must be after start date.");
        
        RuleFor(x => x.AcademicYear.Status)
            .IsInEnum().WithMessage("Invalid status.");
    }
}
