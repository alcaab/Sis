using Desyco.Dms.Application.EducationalLevels.Commands;
using FluentValidation;

namespace Desyco.Dms.Application.EducationalLevels.Validators;

public class UpdateEducationalLevelCommandValidator : AbstractValidator<UpdateEducationalLevelCommand>
{
    public UpdateEducationalLevelCommandValidator()
    {
        RuleFor(x => x.EducationalLevel.Id)
            .GreaterThan(0).WithMessage("Id is required.");

        RuleFor(x => x.EducationalLevel).SetValidator(new EducationalLevelDtoValidator());
    }
}
