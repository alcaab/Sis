using Desyco.Dms.Domain.Evaluations;
using Desyco.Dms.Domain.Evaluations.Interfaces;

namespace Desyco.Dms.Infrastructure.Evaluations.Repositories;

public class EvaluationPeriodRepository(ApplicationDbContext context) : RepositoryBase<EvaluationPeriodEntity, int>(context), IEvaluationPeriodRepository
{
    public async Task<List<EvaluationPeriodEntity>> GetByAcademicYearIdAsync(int academicYearId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(x => x.AcademicYearId == academicYearId)
            .OrderBy(x => x.Sequence)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }
}