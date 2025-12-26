using Desyco.Dms.Application.EducationalLevels.DTOs;
using Desyco.Dms.Domain.EducationalLevels;
using Riok.Mapperly.Abstractions;
using Scrima.Core.Query;

namespace Desyco.Dms.Application.EducationalLevels.Mappers;

[Mapper]
public partial class EducationalLevelMapper
{
    public partial EducationalLevelDto Map(EducationalLevelEntity entity);
    
    public partial EducationalLevelEntity Map(EducationalLevelDto dto);
    
    public partial void Map(EducationalLevelDto dto, EducationalLevelEntity entity);
    
    public partial QueryResult<EducationalLevelDto> Map(QueryResult<EducationalLevelEntity> queryResult);
}
