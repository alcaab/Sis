using FluentValidation;

namespace Desyco.Dms.Application.DaysOfWeek.Validators;

public class UpdateDayOfWeekCommandValidator : AbstractValidator<UpdateDayOfWeekCommand>
{
    public UpdateDayOfWeekCommandValidator()
    {
        RuleFor(x => x.Id).IsInEnum();

        When(x => x.IsSchoolDay, () =>
        {
            RuleFor(x => x.OpenTime).NotNull().WithMessage("Open Time is required for school days.");
            RuleFor(x => x.StartTime).NotNull().WithMessage("Start Time is required for school days.");
            RuleFor(x => x.EndTime).NotNull().WithMessage("End Time is required for school days.");
            RuleFor(x => x.CloseTime).NotNull().WithMessage("Close Time is required for school days.");

            RuleFor(x => x)
                .Must(x => x.StartTime < x.EndTime)
                .When(x => x.StartTime.HasValue && x.EndTime.HasValue)
                .WithMessage("Start Time must be before End Time.");
            
            RuleFor(x => x)
                .Must(x => x.OpenTime <= x.StartTime)
                .When(x => x.OpenTime.HasValue && x.StartTime.HasValue)
                .WithMessage("Open Time must be before or equal to Start Time.");

            RuleFor(x => x)
                .Must(x => x.CloseTime >= x.EndTime)
                .When(x => x.CloseTime.HasValue && x.EndTime.HasValue)
                .WithMessage("Close Time must be after or equal to End Time.");
        });
    }
}
