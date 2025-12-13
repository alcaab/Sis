using Desyco.Dms.Domain.GradingScales;
using Desyco.Dms.Infrastructure.Common.Seeding;
using Microsoft.Extensions.Logging;

namespace Desyco.Dms.Infrastructure.Seeders;

public class GradingScaleTypeSeeder(ApplicationDbContext context, ILogger<GradingScaleTypeSeeder> logger) 
    : EnumEntitySeeder<GradingScaleTypeEntity, GradingScaleType>(context, logger)
{
    protected override GradingScaleTypeEntity CreateEntity(GradingScaleType id) => new() { Id = id };
}
