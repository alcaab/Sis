using Desyco.Mediator.Contracts;
using Desyco.Dms.Application.SpecialDays.DTOs;

namespace Desyco.Dms.Application.SpecialDays.Queries;

public record GetSpecialDayByIdQuery(int Id) : IRequest<SpecialDayDto?>;
