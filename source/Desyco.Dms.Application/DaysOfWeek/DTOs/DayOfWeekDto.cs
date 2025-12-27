namespace Desyco.Dms.Application.DaysOfWeek.DTOs;

public record DayOfWeekDto
{
    public DayOfWeek Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string ShortName { get; init; } = string.Empty;
    public TimeOnly? OpenTime { get; init; }
    public TimeOnly? StartTime { get; init; }
    public TimeOnly? EndTime { get; init; }
    public TimeOnly? CloseTime { get; init; }
    public bool IsStartOfWeek { get; init; }
    public bool IsEndOfWeek { get; init; }
    public bool IsSchoolDay { get; init; }
}
