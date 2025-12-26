using Desyco.Dms.Application.DaysOfWeek.DTOs;
using Desyco.Mediator.Contracts;

namespace Desyco.Dms.Application.DaysOfWeek.Queries;

public record GetAllDaysOfWeekQuery : IRequest<List<DayOfWeekDto>>;
