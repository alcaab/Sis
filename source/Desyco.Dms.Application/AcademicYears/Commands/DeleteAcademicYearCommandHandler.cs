using Desyco.Dms.Application.Common;
using Desyco.Dms.Domain.AcademicYears.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.AcademicYears.Commands;

public class DeleteAcademicYearCommandHandler(IAcademicYearRepository academicYearRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteAcademicYearCommand>
{
    public async Task Handle(DeleteAcademicYearCommand request, CancellationToken cancellationToken = default)
    {
        var academicYear = await academicYearRepository.GetByIdAsync(request.Id, cancellationToken);

        if (academicYear == null)
        {
            // TODO: Replace with specific NotFoundException
            throw new Exception($"AcademicYear with Id {request.Id} not found.");
        }

        await academicYearRepository.DeleteAsync(academicYear, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
