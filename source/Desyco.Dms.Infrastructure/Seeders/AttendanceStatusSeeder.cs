using Desyco.Dms.Domain.Attendances;
using Desyco.Dms.Infrastructure.Common.Seeding;
using Microsoft.Extensions.Logging;

namespace Desyco.Dms.Infrastructure.Seeders;

public class AttendanceStatusSeeder(ApplicationDbContext context, ILogger<AttendanceStatusSeeder> logger) 
    : EnumEntitySeeder<AttendanceStatusEntity, AttendanceStatus>(context, logger)
{
    protected override AttendanceStatusEntity CreateEntity(AttendanceStatus id) => new() { Id = id };
}
