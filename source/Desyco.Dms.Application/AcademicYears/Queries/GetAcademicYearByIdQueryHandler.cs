using Desyco.Dms.Application.AcademicYears.DTOs;
using Desyco.Dms.Application.AcademicYears.Mappers;
using Desyco.Dms.Domain.AcademicYears.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.AcademicYears.Queries;

public class GetAcademicYearByIdQueryHandler(IAcademicYearRepository academicYearRepository, AcademicYearMapper mapper)
    : IRequestHandler<GetAcademicYearByIdQuery, AcademicYearDto>
{
    public async Task<AcademicYearDto> Handle(GetAcademicYearByIdQuery request, CancellationToken cancellationToken = default)
    {
        var academicYear = await academicYearRepository.GetByIdAsync(request.Id, cancellationToken);
        
        return academicYear == null ? throw
            // TODO: Handle not found exception, possibly a custom NotFoundException
            new Exception($"AcademicYear with Id {request.Id} not found.") : mapper.Map(academicYear);
    }
}
