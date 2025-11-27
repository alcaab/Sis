namespace Desyco.Dms.Domain.Scholarships;

public class ScholarshipAwardEntity : EntityBase<int>
{
    public int EnrollmentId { get; set; }

    public int ScholarshipTypeId { get; set; }

    public decimal AssignedPercentage { get; set; }

    public DateTime GrantedDate { get; set; }

    public string? Justification { get; set; }
}
