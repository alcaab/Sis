using Desyco.Dms.Application.AcademicYears.DTOs;
using Desyco.Dms.Application.AcademicYears.Mappers;
using Desyco.Dms.Domain.AcademicYears.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.AcademicYears.Queries;

public class GetAllAcademicYearsQueryHandler(IAcademicYearRepository academicYearRepository, AcademicYearMapper mapper)
    : IRequestHandler<GetAllAcademicYearsQuery, List<AcademicYearDto>>
{
    public async Task<List<AcademicYearDto>> Handle(GetAllAcademicYearsQuery request, CancellationToken cancellationToken = default)
    {
        var academicYears = await academicYearRepository.ListAsync(cancellationToken: cancellationToken);
        return academicYears.Select(mapper.Map).ToList();
    }
}
