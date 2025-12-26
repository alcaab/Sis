using Desyco.Dms.Application.DaysOfWeek.DTOs;
using Desyco.Dms.Domain.Common.Entities;
using Riok.Mapperly.Abstractions;

namespace Desyco.Dms.Application.DaysOfWeek.Mappers;

[Mapper]
public partial class DayOfWeekMapper
{
    public partial DayOfWeekDto ToDto(DayOfWeekEntity entity);
    public partial DayOfWeekEntity ToEntity(DayOfWeekDto dto);
    
    public partial List<DayOfWeekDto> ToDtoList(List<DayOfWeekEntity> entities);
    
    public partial void UpdateEntity(UpdateDayOfWeekCommand command, DayOfWeekEntity entity);
}
