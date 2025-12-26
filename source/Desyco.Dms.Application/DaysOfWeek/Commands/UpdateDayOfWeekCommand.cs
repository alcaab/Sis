using Desyco.Dms.Application.DaysOfWeek.DTOs;
using Desyco.Mediator.Contracts;

namespace Desyco.Dms.Application.DaysOfWeek;

public record UpdateDayOfWeekCommand(
    DayOfWeek Id,
    bool IsSchoolDay,
    TimeOnly? OpenTime,
    TimeOnly? StartTime,
    TimeOnly? EndTime,
    TimeOnly? CloseTime
) : IRequest<DayOfWeekDto>;
