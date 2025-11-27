namespace Desyco.Dms.Domain.Enrollments;

public class EnrollmentEntity : EntityBase<int>
{
    public int AcademicYearId { get; set; }
    
    public int StudentId { get; set; }
    
    public int SectionId { get; set; }

    public DateTime EnrollmentDate { get; set; }
    
    public EnrollmentStatus Status { get; set; }

    public bool IsRepeater { get; set; }

    public bool IsNewStudent { get; set; }

    public bool DocumentsVerified { get; set; }

    public DateTime? WithdrawalDate { get; set; }

    public string? WithdrawalReason { get; set; }
}

