namespace Desyco.Dms.Domain.Common.Entities;

public class DayOfWeekEntity : EntityBase<DayOfWeek>
{
    public string Name { get; set; } = string.Empty;
    
    public string ShortName { get; set; } = string.Empty;

    public TimeOnly? OpenTime { get; set; }
    
    public TimeOnly? StartTime { get; set; }
    
    public TimeOnly? EndTime { get; set; }
    
    public TimeOnly? CloseTime { get; set; }
    
    public bool IsStartOfWeek { get; set; }
    
    public bool IsEndOfWeek { get; set; }
    
    public bool IsSchoolDay { get; set; }
}
