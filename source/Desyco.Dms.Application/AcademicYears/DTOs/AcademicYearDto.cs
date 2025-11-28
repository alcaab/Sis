namespace Desyco.Dms.Application.AcademicYears.DTOs;

public record AcademicYearDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public bool IsActive { get; init; }
}
