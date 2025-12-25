using Desyco.Dms.Application.Evaluations.DTOs;
using Desyco.Dms.Application.Evaluations.Mappers;
using Desyco.Dms.Domain.Evaluations.Interfaces;
using Desyco.Mediator;
using Scrima.Core.Query;

namespace Desyco.Dms.Application.Evaluations.Queries;

public class GetAllEvaluationPeriodsQueryHandler(IEvaluationPeriodRepository repository, EvaluationPeriodMapper mapper)
    : IRequestHandler<GetAllEvaluationPeriodsQuery, QueryResult<EvaluationPeriodDto>>
{
    public async Task<QueryResult<EvaluationPeriodDto>> Handle(GetAllEvaluationPeriodsQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetResultListAsync(
            request.QueryOptions,
            (e, searchTerm) => e.Name.Contains(searchTerm) || e.ShortName.Contains(searchTerm),
            cancellationToken);

        return mapper.Map(result);
    }
}
