using Desyco.Dms.Application.Common;
using Desyco.Dms.Application.Evaluations.DTOs;
using Desyco.Dms.Application.Evaluations.Mappers;
using Desyco.Dms.Domain.Evaluations.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.Evaluations.Commands;

public class CreateEvaluationPeriodCommandHandler(
    IEvaluationPeriodRepository repository,
    IUnitOfWork unitOfWork,
    EvaluationPeriodMapper mapper)
    : IRequestHandler<CreateEvaluationPeriodCommand, EvaluationPeriodDto>
{
    public async Task<EvaluationPeriodDto> Handle(CreateEvaluationPeriodCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map(request.EvaluationPeriod);
        
        await repository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map(entity);
    }
}
