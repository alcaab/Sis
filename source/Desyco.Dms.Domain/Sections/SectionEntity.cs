using Desyco.Dms.Domain.Shifts;

namespace Desyco.Dms.Domain.Sections;

public class SectionEntity: EntityBase<int>
{
    public int AcademicYearId { get; set; }
    
    public int GradeId { get; set; }
    
    public ShiftType ShiftId { get; set; }
    
    public int? ClassroomId { get; set; }
    
    public int? LeadTeacherId { get; set; }
    
    public string Name { get; set; }  = string.Empty; 
    
    public int Capacity { get; set; }
    
    public bool IsActive { get; set; }
}
