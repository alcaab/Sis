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
        // Ensure they are ordered by ID (Sunday=0, Monday=1, etc or however DayOfWeek enum works)
        // System.DayOfWeek: Sunday = 0, Monday = 1, ..., Saturday = 6
        var orderedEntities = entities.OrderBy(x => x.Id).ToList();
        return mapper.ToDtoList(orderedEntities);
    }
}
