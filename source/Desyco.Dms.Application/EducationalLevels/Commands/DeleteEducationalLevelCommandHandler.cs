using Desyco.Dms.Application.Common;
using Desyco.Dms.Application.Common.Exceptions;
using Desyco.Dms.Domain.EducationalLevels.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.EducationalLevels.Commands;

public class DeleteEducationalLevelCommandHandler(
    IEducationalLevelRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteEducationalLevelCommand>
{
    public async Task Handle(DeleteEducationalLevelCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"EducationalLevel with Id {request.Id} was not found.");
        }

        await repository.DeleteAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
