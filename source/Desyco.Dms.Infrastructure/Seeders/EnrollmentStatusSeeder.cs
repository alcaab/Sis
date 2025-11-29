using Desyco.Dms.Domain.Enrollments;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Seeding;
using Microsoft.Extensions.Logging;

namespace Desyco.Dms.Infrastructure.Seeders;

public class EnrollmentStatusSeeder(ApplicationDbContext context, ILogger<EnrollmentStatusSeeder> logger) 
    : EnumEntitySeeder<EnrollmentStatusEntity, EnrollmentStatus>(context, logger)
{
    protected override EnrollmentStatusEntity CreateEntity(EnrollmentStatus id) => new() { Id = id };
}
