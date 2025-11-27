namespace Desyco.Dms.Domain.Grades;

public class GradeEntity : EntityBase<int>
{
    public int EducationLevelId { get; set; }

    public string Name { get; set; } = string.Empty;
}
