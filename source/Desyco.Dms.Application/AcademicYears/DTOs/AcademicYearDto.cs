using Desyco.Dms.Domain.AcademicYears;

namespace Desyco.Dms.Application.AcademicYears.DTOs;

public record AcademicYearDto
{
    public int Id { get; init; }
    
    public string Name { get; init; } = string.Empty;
    
    public DateOnly? StartDate { get; init; }
    
    public DateOnly? EndDate { get; init; }
    
    public AcademicYearStatus Status { get; init; }
}