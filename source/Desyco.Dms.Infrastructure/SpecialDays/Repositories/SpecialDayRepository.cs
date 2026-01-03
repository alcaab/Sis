using Desyco.Dms.Domain.Common.Entities;
using Desyco.Dms.Domain.EducationalLevels;
using Desyco.Dms.Domain.SpecialDays;
using Desyco.Dms.Domain.SpecialDays.Interfaces;
using Desyco.Dms.Domain.Evaluations;

namespace Desyco.Dms.Infrastructure.SpecialDays.Repositories;

public class SpecialDayRepository(ApplicationDbContext context) : RepositoryBase<SpecialDayEntity, int>(context), ISpecialDayRepository
{
    public async Task<IReadOnlyList<EvaluationPeriodSpecialDay>> GetByAcademicYearIdAsync(int academicYearId,
        CancellationToken cancellationToken = default)
    {
        var query = from ep in Context.Set<EvaluationPeriodEntity>()
            join el in Context.Set<EducationalLevelEntity>() on ep.LevelTypeId equals el.Id
            join sd in Context.Set<SpecialDayEntity>() on ep.Id equals sd.EvaluationPeriodId into lsd
            from sd in lsd.DefaultIfEmpty()
            where ep.AcademicYearId == academicYearId
            select new EvaluationPeriodSpecialDay(ep, el, sd);

        return await query.ToListAsync(cancellationToken);
    }
}
