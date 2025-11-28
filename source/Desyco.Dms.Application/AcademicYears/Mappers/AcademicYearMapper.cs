using Desyco.Dms.Application.AcademicYears.DTOs;
using Desyco.Dms.Domain.AcademicYears;
using Riok.Mapperly.Abstractions;

namespace Desyco.Dms.Application.AcademicYears.Mappers;

[Mapper]
public partial class AcademicYearMapper
{
    public partial AcademicYearDto Map(AcademicYearEntity academicYear);
    public partial void Map(AcademicYearDto dto, AcademicYearEntity academicYear);
}
