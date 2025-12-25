using Desyco.Dms.Domain.Common.Interfaces;

namespace Desyco.Dms.Domain.Evaluations.Interfaces;

public interface IEvaluationPeriodRepository : IRepositoryBase<EvaluationPeriodEntity, int>
{
    Task<List<EvaluationPeriodEntity>> GetByAcademicYearIdAsync(int academicYearId, CancellationToken cancellationToken = default);
}
