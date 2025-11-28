using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Desyco.Dms.Infrastructure.Common;

[ExcludeFromCodeCoverage]
public static class EfConfigurationExtensions
{
    private const string DateTimeDefault = "datetime2(0)";

    public static PropertyBuilder HasDefaultDateTime(this PropertyBuilder source)
        => source.HasColumnType(DateTimeDefault);

    public static PropertyBuilder HasMoneyValuePrecision(this PropertyBuilder source)
        => source.HasPrecision(14, 2);

    public static PropertyBuilder HasEvaluationValuePrecision(this PropertyBuilder source)
        => source.HasPrecision(5, 2);
}
