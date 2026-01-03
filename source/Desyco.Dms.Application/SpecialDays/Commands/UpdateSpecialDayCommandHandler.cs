using Desyco.Dms.Application.Common.Exceptions;
using Desyco.Dms.Application.SpecialDays.DTOs;
using Desyco.Dms.Application.SpecialDays.Mappers;
using Desyco.Dms.Application.Common;
using Desyco.Dms.Domain.SpecialDays;
using Desyco.Dms.Domain.SpecialDays.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.SpecialDays.Commands;

public class UpdateSpecialDayCommandHandler(
    ISpecialDayRepository specialDayRepository,
    SpecialDayMapper mapper,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateSpecialDayCommand, SpecialDayDto>
{
    public async Task<SpecialDayDto> Handle(UpdateSpecialDayCommand request, CancellationToken cancellationToken = default)
    {
        var specialDay = await specialDayRepository.GetByIdAsync(request.SpecialDay.Id, cancellationToken);
        
        if (specialDay == null)
        {
            throw new NotFoundException($"Entity \"{nameof(SpecialDayEntity)}\" ({request.SpecialDay.Id}) was not found.");
        }
        
        mapper.Map(request.SpecialDay, specialDay);

        await specialDayRepository.UpdateAsync(specialDay, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map(specialDay);
    }
}
