using Desyco.Dms.Application.SpecialDays.DTOs;
using Desyco.Dms.Application.SpecialDays.Mappers;
using Desyco.Dms.Domain.SpecialDays.Interfaces;
using Desyco.Mediator;
using Scrima.Core.Query;

namespace Desyco.Dms.Application.SpecialDays.Queries;

public class GetSpecialDaysListQueryHandler(ISpecialDayRepository specialDayRepository, SpecialDayMapper mapper)
    : IRequestHandler<GetSpecialDaysListQuery, QueryResult<SpecialDayDto>>
{
    public async Task<QueryResult<SpecialDayDto>> Handle(GetSpecialDaysListQuery request, CancellationToken cancellationToken = default)
    {
        var result = await specialDayRepository.GetResultListAsync(
            request.QueryOptions,
            (e, searchTerm) => e.Name.Contains(searchTerm) || e.Description.Contains(searchTerm),
            cancellationToken);
            
        return mapper.Map(result);
    }
}
