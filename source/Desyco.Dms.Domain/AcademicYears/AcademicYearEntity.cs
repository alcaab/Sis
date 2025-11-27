namespace Desyco.Dms.Domain.AcademicYears;

public class AcademicYearEntity : EntityBase<int>
{
    public string Name { get; set; } = string.Empty;

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool IsActive { get; set; }
}
