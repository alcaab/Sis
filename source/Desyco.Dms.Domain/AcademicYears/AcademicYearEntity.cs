using Desyco.Dms.Domain.Common.Attributes;

namespace Desyco.Dms.Domain.AcademicYears;

public class AcademicYearEntity : EntityBase<int>
{
    [OrderByProp]
    public string Name { get; set; } = string.Empty;

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool IsActive { get; set; }
}
