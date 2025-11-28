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
        
        if (academicYear == null)
        {
            // TODO: Handle not found exception, possibly a custom NotFoundException
            throw new Exception($"AcademicYear with Id {request.Id} not found.");
        }

        return mapper.Map(academicYear);
    }
}
