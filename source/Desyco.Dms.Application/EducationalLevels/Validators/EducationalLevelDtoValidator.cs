using Desyco.Dms.Application.EducationalLevels.DTOs;
using FluentValidation;

namespace Desyco.Dms.Application.EducationalLevels.Validators;

public class EducationalLevelDtoValidator : AbstractValidator<EducationalLevelDto>
{
    public EducationalLevelDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.LevelTypeId)
            .IsInEnum().WithMessage("Invalid Educational Level Type.");
    }
}
