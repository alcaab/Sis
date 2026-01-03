using Desyco.Dms.Application.SpecialDays.DTOs;
using Desyco.Dms.Application.SpecialDays.Mappers;
using Desyco.Dms.Domain.SpecialDays.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.SpecialDays.Queries;

public class GetSpecialDayByIdQueryHandler(ISpecialDayRepository specialDayRepository, SpecialDayMapper mapper)
    : IRequestHandler<GetSpecialDayByIdQuery, SpecialDayDto?>
{
    public async Task<SpecialDayDto?> Handle(GetSpecialDayByIdQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await specialDayRepository.GetByIdAsync(request.Id, cancellationToken);
        return entity == null ? null : mapper.Map(entity);
    }
}
