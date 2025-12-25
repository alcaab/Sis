using Desyco.Dms.Application.Common;
using Desyco.Dms.Application.Common.Exceptions;
using Desyco.Dms.Application.Evaluations.DTOs;
using Desyco.Dms.Application.Evaluations.Mappers;
using Desyco.Dms.Domain.Evaluations.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.Evaluations.Commands;

public class UpdateEvaluationPeriodCommandHandler(
    IEvaluationPeriodRepository repository,
    IUnitOfWork unitOfWork,
    EvaluationPeriodMapper mapper)
    : IRequestHandler<UpdateEvaluationPeriodCommand, EvaluationPeriodDto>
{
    public async Task<EvaluationPeriodDto> Handle(UpdateEvaluationPeriodCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.EvaluationPeriod.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"EvaluationPeriod with Id {request.EvaluationPeriod.Id} was not found.");
        }

        mapper.Map(request.EvaluationPeriod, entity);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map(entity);
    }
}
