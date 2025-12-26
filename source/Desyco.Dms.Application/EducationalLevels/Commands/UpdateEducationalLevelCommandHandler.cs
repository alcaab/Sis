using Desyco.Dms.Application.Common;
using Desyco.Dms.Application.Common.Exceptions;
using Desyco.Dms.Application.EducationalLevels.DTOs;
using Desyco.Dms.Application.EducationalLevels.Mappers;
using Desyco.Dms.Domain.EducationalLevels.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.EducationalLevels.Commands;

public class UpdateEducationalLevelCommandHandler(
    IEducationalLevelRepository repository,
    IUnitOfWork unitOfWork,
    EducationalLevelMapper mapper)
    : IRequestHandler<UpdateEducationalLevelCommand, EducationalLevelDto>
{
    public async Task<EducationalLevelDto> Handle(UpdateEducationalLevelCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.EducationalLevel.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"EducationalLevel with Id {request.EducationalLevel.Id} was not found.");
        }

        mapper.Map(request.EducationalLevel, entity);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map(entity);
    }
}
