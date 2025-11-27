namespace Desyco.Dms.Domain.Attendances;

public class AttendanceEntity: EntityBase<int>
{
    public int EnrollmentId { get; set; }

    public DateTime Date { get; set; }

    public AttendanceStatus Status { get; set; }

    public string? Remarks { get; set; }
}
