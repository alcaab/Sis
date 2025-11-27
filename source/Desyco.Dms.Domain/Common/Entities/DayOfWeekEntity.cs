namespace Desyco.Dms.Domain.Common.Entities;

public class DayOfWeekEntity : EntityBase<DayOfWeek>, ITranslationKey
{
    public bool IsStartOfWeek { get; set; }
    
    public string? TranslationKey { get; set; }
}
