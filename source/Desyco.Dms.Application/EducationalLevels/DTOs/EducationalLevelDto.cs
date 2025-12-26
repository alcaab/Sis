using Desyco.Dms.Domain.EducationalLevels;

namespace Desyco.Dms.Application.EducationalLevels.DTOs;

public class EducationalLevelDto
{
    public int Id { get; set; }
    public EducationalLevelType LevelTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
