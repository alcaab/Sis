using Desyco.Dms.Domain.Common.Entities;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Seeding;
using Microsoft.Extensions.Logging;

namespace Desyco.Dms.Infrastructure.Seeders;

public class DayOfWeekSeeder(ApplicationDbContext context, ILogger<DayOfWeekSeeder> logger) 
    : EnumEntitySeeder<DayOfWeekEntity, DayOfWeek>(context, logger)
{
    protected override DayOfWeekEntity CreateEntity(DayOfWeek id) => new() { Id = id };
}
