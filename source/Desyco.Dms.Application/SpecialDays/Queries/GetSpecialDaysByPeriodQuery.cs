using Desyco.Mediator.Contracts;
using Desyco.Dms.Application.SpecialDays.DTOs;

namespace Desyco.Dms.Application.SpecialDays.Queries;

public record GetSpecialDaysByPeriodQuery(int EvaluationPeriodId) : IRequest<List<SpecialDayDto>>;
