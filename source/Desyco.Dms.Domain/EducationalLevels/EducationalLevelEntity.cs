namespace Desyco.Dms.Domain.EducationalLevels;

public class EducationalLevelEntity : EntityBase<int>
{
    public EducationalLevelType LevelTypeId { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
