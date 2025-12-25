using Desyco.Dms.Application.Evaluations.DTOs;
using Desyco.Dms.Application.Evaluations.Mappers;
using Desyco.Dms.Domain.Evaluations.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.Evaluations.Queries;

public class GetEvaluationPeriodsByAcademicYearQueryHandler(
    IEvaluationPeriodRepository repository,
    EvaluationPeriodMapper mapper)
    : IRequestHandler<GetEvaluationPeriodsByAcademicYearQuery, List<EvaluationPeriodViewDto>>
{
    public async Task<List<EvaluationPeriodViewDto>> Handle(GetEvaluationPeriodsByAcademicYearQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetByAcademicYearIdAsync(request.AcademicYearId, cancellationToken);
        return entities.Select(mapper.MapToView).ToList();
    }
}
