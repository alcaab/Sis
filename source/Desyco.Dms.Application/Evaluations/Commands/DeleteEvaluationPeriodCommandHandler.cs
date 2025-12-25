using Desyco.Dms.Application.Common;
using Desyco.Dms.Application.Common.Exceptions;
using Desyco.Dms.Domain.Evaluations.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.Evaluations.Commands;

public class DeleteEvaluationPeriodCommandHandler(
    IEvaluationPeriodRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteEvaluationPeriodCommand>
{
    public async Task Handle(DeleteEvaluationPeriodCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"EvaluationPeriod with Id {request.Id} was not found.");
        }

        await repository.DeleteAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
