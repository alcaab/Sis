using Desyco.Dms.Application.Common;
using Desyco.Dms.Application.DaysOfWeek.DTOs;
using Desyco.Dms.Application.DaysOfWeek.Mappers;
using Desyco.Dms.Domain.Common.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.DaysOfWeek.Commands;

public class UpdateDayOfWeekCommandHandler(IDayOfWeekRepository repository, DayOfWeekMapper mapper, IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateDayOfWeekCommand, DayOfWeekDto>
{
    public async Task<DayOfWeekDto> Handle(UpdateDayOfWeekCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            throw new KeyNotFoundException($"DayOfWeek with ID {request.Id} not found.");
        }

        mapper.UpdateEntity(request, entity);
        
        // If not school day, clear times to ensure consistency? 
        // Or rely on the validator enforcing presence if true, but if false, we might want to clear them.
        // The requirements say: "Si 'School Day' es 'No', los campos de hora deben ocultarse o deshabilitarse."
        // It doesn't explicitly say clear them in DB, but it's good practice.
        if (!request.IsSchoolDay)
        {
            entity.OpenTime = null;
            entity.StartTime = null;
            entity.EndTime = null;
            entity.CloseTime = null;
        }

        await repository.UpdateAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.ToDto(entity);
    }
}
