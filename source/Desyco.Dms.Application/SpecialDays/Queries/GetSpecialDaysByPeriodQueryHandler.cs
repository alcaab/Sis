using Desyco.Dms.Application.SpecialDays.DTOs;
using Desyco.Dms.Application.SpecialDays.Mappers;
using Desyco.Dms.Domain.SpecialDays.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.SpecialDays.Queries;

public class GetSpecialDaysByPeriodQueryHandler(ISpecialDayRepository specialDayRepository, SpecialDayMapper mapper)
    : IRequestHandler<GetSpecialDaysByPeriodQuery, List<SpecialDayDto>>
{
    public async Task<List<SpecialDayDto>> Handle(GetSpecialDaysByPeriodQuery request, CancellationToken cancellationToken = default)
    {
        var list = await specialDayRepository.ListAsync(
            filter: x => x.EvaluationPeriodId == request.EvaluationPeriodId,
            cancellationToken: cancellationToken);
            
        return list.Select(mapper.Map).ToList();
    }
}
