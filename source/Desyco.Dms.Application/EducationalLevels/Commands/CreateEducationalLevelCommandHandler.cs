using Desyco.Dms.Application.Common;
using Desyco.Dms.Application.EducationalLevels.DTOs;
using Desyco.Dms.Application.EducationalLevels.Mappers;
using Desyco.Dms.Domain.EducationalLevels.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.EducationalLevels.Commands;

public class CreateEducationalLevelCommandHandler(
    IEducationalLevelRepository repository,
    IUnitOfWork unitOfWork,
    EducationalLevelMapper mapper)
    : IRequestHandler<CreateEducationalLevelCommand, EducationalLevelDto>
{
    public async Task<EducationalLevelDto> Handle(CreateEducationalLevelCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map(request.EducationalLevel);
        
        await repository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map(entity);
    }
}
