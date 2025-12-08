using Desyco.Dms.Application.AcademicYears.DTOs;
using Desyco.Dms.Application.AcademicYears.Mappers;
using Desyco.Dms.Domain.AcademicYears.Interfaces;
using Desyco.Mediator;
using Scrima.Core.Query;

namespace Desyco.Dms.Application.AcademicYears.Queries;

public class GetAllAcademicYearsQueryHandler(IAcademicYearRepository academicYearRepository, AcademicYearMapper mapper)
    : IRequestHandler<GetAllAcademicYearsQuery, QueryResult<AcademicYearDto>>
{
    public async Task<QueryResult<AcademicYearDto>> Handle(GetAllAcademicYearsQuery request,
        CancellationToken cancellationToken = default)
    {
        var result =
            await academicYearRepository.GetResultListAsync(
                request.QueryOptions,
                (e,searchTerm) => e.Name.StartsWith(searchTerm),
                cancellationToken);
        
        return mapper.Map(result);
    }
}
