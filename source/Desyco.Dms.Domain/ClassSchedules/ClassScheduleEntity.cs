namespace Desyco.Dms.Domain.ClassSchedules;

public class ClassScheduleEntity : EntityBase<int>
{
    public int AssignmentId { get; set; }

    public DayOfWeek DayOfWeekId { get; set; }
    
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }

    public int? ClassroomId { get; set; }
}
