using Desyco.Dms.Application.SpecialDays.DTOs;
using Desyco.Dms.Application.SpecialDays.Mappers;
using Desyco.Dms.Application.Common;
using Desyco.Dms.Domain.SpecialDays;
using Desyco.Dms.Domain.SpecialDays.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.SpecialDays.Commands;

public class CreateSpecialDayCommandHandler(
    ISpecialDayRepository specialDayRepository,
    SpecialDayMapper mapper,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateSpecialDayCommand, SpecialDayDto>
{
    public async Task<SpecialDayDto> Handle(CreateSpecialDayCommand request, CancellationToken cancellationToken = default)
    {
        var specialDay = new SpecialDayEntity { Id = 0 };
        
        mapper.Map(request.SpecialDay, specialDay);

        specialDay.Id = 0; 

        await specialDayRepository.AddAsync(specialDay, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map(specialDay);
    }
}
