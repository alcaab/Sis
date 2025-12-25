using Desyco.Dms.Application.Common.Exceptions;
using Desyco.Dms.Application.Evaluations.DTOs;
using Desyco.Dms.Application.Evaluations.Mappers;
using Desyco.Dms.Domain.Evaluations.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.Evaluations.Queries;

public class GetEvaluationPeriodByIdQueryHandler(IEvaluationPeriodRepository repository, EvaluationPeriodMapper mapper)
    : IRequestHandler<GetEvaluationPeriodByIdQuery, EvaluationPeriodDto>
{
    public async Task<EvaluationPeriodDto> Handle(GetEvaluationPeriodByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);

        return entity == null
            ? throw new NotFoundException($"EvaluationPeriod with Id {request.Id} was not found.")
            : mapper.Map(entity);
    }
}
