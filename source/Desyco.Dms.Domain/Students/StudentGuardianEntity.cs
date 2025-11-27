namespace Desyco.Dms.Domain.Students;

public class StudentGuardianEntity :  EntityBase<int>
{
    public int StudentId { get; set; }

    public int GuardianId { get; set; }
    
    public int RelationShipId { get; set; }
}
