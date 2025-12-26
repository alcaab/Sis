using Desyco.Dms.Application.EducationalLevels.Commands;
using FluentValidation;

namespace Desyco.Dms.Application.EducationalLevels.Validators;

public class CreateEducationalLevelCommandValidator : AbstractValidator<CreateEducationalLevelCommand>
{
    public CreateEducationalLevelCommandValidator()
    {
        RuleFor(x => x.EducationalLevel).SetValidator(new EducationalLevelDtoValidator());
    }
}
