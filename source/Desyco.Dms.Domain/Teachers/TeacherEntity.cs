namespace Desyco.Dms.Domain.Teachers;

public class TeacherEntity: EntityBase<int>
{
    public string Number { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateOnly? HireDate { get; set; }
}
