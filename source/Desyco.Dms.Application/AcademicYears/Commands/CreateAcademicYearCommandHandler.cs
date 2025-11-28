using Desyco.Dms.Application.AcademicYears.DTOs;
using Desyco.Dms.Application.AcademicYears.Mappers;
using Desyco.Dms.Application.Common;
using Desyco.Dms.Domain.AcademicYears;
using Desyco.Dms.Domain.AcademicYears.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.AcademicYears.Commands;

public class CreateAcademicYearCommandHandler(
    IAcademicYearRepository academicYearRepository,
    AcademicYearMapper mapper,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateAcademicYearCommand, AcademicYearDto>
{
    public async Task<AcademicYearDto> Handle(CreateAcademicYearCommand request, CancellationToken cancellationToken = default)
    {
        var academicYear = new AcademicYearEntity
        {
            // Since EntityBase requires Id, and it's likely auto-increment or similar, we usually initialize it to default if needed, 
            // but here we are mapping.
            // However, for creation we usually want to ignore the ID from the DTO if it's set, or ensure it's treated as new.
            Id = 0 
        };
        
        mapper.Map(request.AcademicYear, academicYear);

        // Ensure Id is 0/default to prevent issues if the DTO had a value (unless we want to force ID, which is rare for int keys)
        academicYear.Id = 0; 

        await academicYearRepository.AddAsync(academicYear, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map(academicYear);
    }
}
