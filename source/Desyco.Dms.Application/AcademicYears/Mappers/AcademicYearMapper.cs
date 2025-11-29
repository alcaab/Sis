using Desyco.Dms.Application.AcademicYears.DTOs;
using Desyco.Dms.Domain.AcademicYears;
using Riok.Mapperly.Abstractions;
using Scrima.Core.Query;

namespace Desyco.Dms.Application.AcademicYears.Mappers;

[Mapper]
public partial class AcademicYearMapper
{
    public partial AcademicYearDto Map(AcademicYearEntity academicYear);
    
    public partial void Map(AcademicYearDto dto, AcademicYearEntity academicYear);
    
    public partial QueryResult<AcademicYearDto> Map(QueryResult<AcademicYearEntity> queryResult);
}
