using Desyco.Dms.Application.Common.Exceptions;
using Desyco.Dms.Application.Common;
using Desyco.Dms.Domain.SpecialDays;
using Desyco.Dms.Domain.SpecialDays.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.SpecialDays.Commands;

public class DeleteSpecialDayCommandHandler(
    ISpecialDayRepository specialDayRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteSpecialDayCommand>
{
    public async Task Handle(DeleteSpecialDayCommand request, CancellationToken cancellationToken = default)
    {
        var specialDay = await specialDayRepository.GetByIdAsync(request.Id, cancellationToken);

        if (specialDay == null)
        {
             throw new NotFoundException($"Entity \"{nameof(SpecialDayEntity)}\" ({request.Id}) was not found.");
        }

        await specialDayRepository.DeleteAsync(specialDay, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
