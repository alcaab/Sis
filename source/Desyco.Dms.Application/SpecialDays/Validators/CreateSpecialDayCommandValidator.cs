using Desyco.Dms.Application.SpecialDays.Commands;
using FluentValidation;

namespace Desyco.Dms.Application.SpecialDays.Validators;

public class CreateSpecialDayCommandValidator : AbstractValidator<CreateSpecialDayCommand>
{
    public CreateSpecialDayCommandValidator()
    {
        RuleFor(x => x.SpecialDay).NotNull();
        RuleFor(x => x.SpecialDay.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.SpecialDay.Date).NotEmpty();
        RuleFor(x => x.SpecialDay.Type).IsInEnum();
    }
}
