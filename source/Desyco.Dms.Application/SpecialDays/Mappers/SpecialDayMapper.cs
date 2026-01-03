using Desyco.Dms.Application.SpecialDays.DTOs;
using Desyco.Dms.Domain.SpecialDays;
using Riok.Mapperly.Abstractions;
using Scrima.Core.Query;

namespace Desyco.Dms.Application.SpecialDays.Mappers;

[Mapper]
public partial class SpecialDayMapper
{
    public partial SpecialDayDto Map(SpecialDayEntity entity);
    
    public partial SpecialDayEntity Map(SpecialDayDto dto);
    
    public partial void Map(SpecialDayDto dto, SpecialDayEntity entity);
    
    public partial QueryResult<SpecialDayDto> Map(QueryResult<SpecialDayEntity> result);
}
