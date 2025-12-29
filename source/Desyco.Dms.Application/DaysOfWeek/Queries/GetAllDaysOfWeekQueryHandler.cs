using Desyco.Dms.Application.DaysOfWeek.DTOs;
using Desyco.Dms.Application.DaysOfWeek.Mappers;
using Desyco.Dms.Domain.Common.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.DaysOfWeek.Queries;

public class GetAllDaysOfWeekQueryHandler(IDayOfWeekRepository repository, DayOfWeekMapper mapper) 
    : IRequestHandler<GetAllDaysOfWeekQuery, List<DayOfWeekDto>>
{
    public async Task<List<DayOfWeekDto>> Handle(GetAllDaysOfWeekQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.ListAsync(cancellationToken: cancellationToken);
        var firstItem = entities.FirstOrDefault(d => d.IsStartOfWeek);

        var orderedEntities = firstItem is null
            ? entities.OrderBy(e => e.Id).ToList()
            : entities .OrderBy(e => (e.Id - firstItem.Id + 7) % 7).ToList();
        
        return mapper.ToDtoList(orderedEntities);
    }
}
