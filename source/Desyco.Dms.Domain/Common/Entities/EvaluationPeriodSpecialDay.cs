using Desyco.Dms.Domain.EducationalLevels;
using Desyco.Dms.Domain.Evaluations;
using Desyco.Dms.Domain.SpecialDays;

namespace Desyco.Dms.Domain.Common.Entities;

public record EvaluationPeriodSpecialDay(
    EvaluationPeriodEntity EvaluationPeriodEntity,
    EducationalLevelEntity EducationalLevelEntity,
    SpecialDayEntity? SpecialDayEntity)
{
    public SpecialDayEntity NotNullSpecialDay
    {
        get => SpecialDayEntity ?? new SpecialDayEntity { Id = 0 };
    }
};
