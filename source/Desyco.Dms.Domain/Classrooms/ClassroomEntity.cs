namespace Desyco.Dms.Domain.Classrooms;

public class ClassroomEntity: EntityBase<int>
{
    public int ClassroomTypeId { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public string? Code { get; set; }

    public string? Building { get; set; }

    public string? Floor { get; set; }

    public int Capacity { get; set; }

    public int RecommendedCapacity { get; set; }
}
