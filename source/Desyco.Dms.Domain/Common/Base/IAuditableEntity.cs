namespace Desyco.Dms.Domain.Common.Base;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }
    
    DateTime? UpdatedAt { get; set; }

    string CreatedBy { get; set; }
    
    string? UpdatedBy { get; set; }
}
