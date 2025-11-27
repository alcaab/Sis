namespace Desyco.Dms.Domain.Students;

public class StudentEntity : EntityBase<int>
{
    public string Number { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateOnly DateOfBirth { get; set; }
    
    public StudentStatus Status { get; set; }
}
