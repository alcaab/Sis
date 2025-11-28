using Desyco.Dms.Application.AcademicYears.DTOs;
using Desyco.Dms.Application.AcademicYears.Mappers;
using Desyco.Dms.Application.Common;
using Desyco.Dms.Domain.AcademicYears.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.AcademicYears.Commands;

public class UpdateAcademicYearCommandHandler(
    IAcademicYearRepository academicYearRepository,
    AcademicYearMapper mapper,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateAcademicYearCommand, AcademicYearDto>
{
    public async Task<AcademicYearDto> Handle(UpdateAcademicYearCommand request, CancellationToken cancellationToken = default)
    {
        var academicYear = await academicYearRepository.GetByIdAsync(request.AcademicYear.Id, cancellationToken);

        if (academicYear == null)
        {
            // TODO: Replace with specific NotFoundException
            throw new Exception($"AcademicYear with Id {request.AcademicYear.Id} not found.");
        }

        mapper.Map(request.AcademicYear, academicYear);

        await academicYearRepository.UpdateAsync(academicYear, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map(academicYear);
    }
}
