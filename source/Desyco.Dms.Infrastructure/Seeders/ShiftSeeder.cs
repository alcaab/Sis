using Desyco.Dms.Domain.Shifts;
using Desyco.Dms.Infrastructure.Common.Seeding;
using Microsoft.Extensions.Logging;

namespace Desyco.Dms.Infrastructure.Seeders;

public class ShiftSeeder(ApplicationDbContext context, ILogger<ShiftSeeder> logger) 
    : EnumEntitySeeder<ShiftEntity, ShiftType>(context, logger)
{
    protected override ShiftEntity CreateEntity(ShiftType id) => new() { Id = id };
}
