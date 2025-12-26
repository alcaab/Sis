using Desyco.Dms.Application.EducationalLevels.DTOs;
using Desyco.Dms.Application.EducationalLevels.Mappers;
using Desyco.Dms.Domain.EducationalLevels.Interfaces;
using Desyco.Mediator;
using Scrima.Core.Query;

namespace Desyco.Dms.Application.EducationalLevels.Queries;

public class GetEducationalLevelListQueryHandler(IEducationalLevelRepository repository, EducationalLevelMapper mapper)
    : IRequestHandler<GetEducationalLevelListQuery, QueryResult<EducationalLevelDto>>
{
    public async Task<QueryResult<EducationalLevelDto>> Handle(GetEducationalLevelListQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetResultListAsync(
            request.QueryOptions,
            (e, searchTerm) => e.Name.Contains(searchTerm),
            cancellationToken);

        return mapper.Map(result);
    }
}
