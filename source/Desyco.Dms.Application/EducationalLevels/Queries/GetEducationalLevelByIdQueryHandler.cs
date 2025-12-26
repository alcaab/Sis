using Desyco.Dms.Application.Common.Exceptions;
using Desyco.Dms.Application.EducationalLevels.DTOs;
using Desyco.Dms.Application.EducationalLevels.Mappers;
using Desyco.Dms.Domain.EducationalLevels.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.EducationalLevels.Queries;

public class GetEducationalLevelByIdQueryHandler(IEducationalLevelRepository repository, EducationalLevelMapper mapper)
    : IRequestHandler<GetEducationalLevelByIdQuery, EducationalLevelDto>
{
    public async Task<EducationalLevelDto> Handle(GetEducationalLevelByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException($"EducationalLevel with Id {request.Id} was not found.");
        }

        return mapper.Map(entity);
    }
}
