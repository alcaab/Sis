using Desyco.Dms.Domain.EducationalLevels;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Seeding;
using Microsoft.Extensions.Logging;

namespace Desyco.Dms.Infrastructure.Seeders;

public class EducationalLevelTypeSeeder(ApplicationDbContext context, ILogger<EducationalLevelTypeSeeder> logger) 
    : EnumEntitySeeder<EducationalLevelTypeEntity, EducationalLevelType>(context, logger)
{
    protected override EducationalLevelTypeEntity CreateEntity(EducationalLevelType id) => new() { Id = id };
}
