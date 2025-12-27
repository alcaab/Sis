using Desyco.Dms.Application.DaysOfWeek.DTOs;
using Desyco.Mediator.Contracts;

namespace Desyco.Dms.Application.DaysOfWeek.Commands;

public record UpdateDayOfWeekCommand(List<DayOfWeekDto> Days) : IRequest<List<DayOfWeekDto>>;
