namespace Desyco.Dms.Domain.Teachers;

public class TeacherAssignmentEntity: EntityBase<int>
{
    public int TeacherId { get; set; }

    public int SectionId { get; set; }

    public int SubjectId { get; set; }
}
