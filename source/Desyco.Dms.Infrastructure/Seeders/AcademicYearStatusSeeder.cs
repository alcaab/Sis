using Desyco.Dms.Domain.AcademicYears;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Seeding;
using Microsoft.Extensions.Logging;

namespace Desyco.Dms.Infrastructure.Seeders;

public class AcademicYearStatusSeeder(ApplicationDbContext context, ILogger<AcademicYearStatusSeeder> logger) 
    : EnumEntitySeeder<AcademicYearStatusEntity, AcademicYearStatus>(context, logger)
{
    protected override AcademicYearStatusEntity CreateEntity(AcademicYearStatus id) => new() { Id = id };
}
