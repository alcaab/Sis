using Desyco.Dms.Application.Common;
using Desyco.Dms.Application.DaysOfWeek.DTOs;
using Desyco.Dms.Application.DaysOfWeek.Mappers;
using Desyco.Dms.Domain.Common.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.DaysOfWeek.Commands;

public class UpdateWeeklyScheduleCommandHandler(IDayOfWeekRepository repository, DayOfWeekMapper mapper, IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateDayOfWeekCommand, List<DayOfWeekDto>>
{
    public async Task<List<DayOfWeekDto>> Handle(UpdateDayOfWeekCommand request, CancellationToken cancellationToken)
    {
        var entities = await repository.ListAsync(cancellationToken: cancellationToken);
        var entitiesMap = entities.ToDictionary(e => e.Id);

        foreach (var dayUpdate in request.Days)
        {
            if (!entitiesMap.TryGetValue(dayUpdate.Id, out var entity))
            {
                continue; 
            }

            mapper.UpdateEntity(dayUpdate, entity);

            // if (!dayUpdate.IsSchoolDay)
            // {
            //     entity.OpenTime = null;
            //     entity.StartTime = null;
            //     entity.EndTime = null;
            //     entity.CloseTime = null;
            // }
            
            // EF Core tracks changes, so no need to call UpdateAsync explicitly for each if tracking is enabled.
            // Repository ListAsync typically returns tracked entities unless configured otherwise.
            // But to be safe and follow pattern:
            await repository.UpdateAsync(entity, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Return updated list
        return mapper.ToDtoList(entities.OrderBy(x => x.Id).ToList());
    }
}
