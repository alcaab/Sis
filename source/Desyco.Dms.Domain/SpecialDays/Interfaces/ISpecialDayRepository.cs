using Desyco.Dms.Domain.Common.Entities;
using Desyco.Dms.Domain.Common.Interfaces;

namespace Desyco.Dms.Domain.SpecialDays.Interfaces;

public interface ISpecialDayRepository : IRepositoryBase<SpecialDayEntity, int>
{
    Task<IReadOnlyList<EvaluationPeriodSpecialDay>> GetByAcademicYearIdAsync(int academicYearId, CancellationToken cancellationToken = default);
}
