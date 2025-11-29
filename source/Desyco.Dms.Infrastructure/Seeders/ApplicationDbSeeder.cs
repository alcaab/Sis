using Desyco.Dms.Infrastructure.Common.Seeding;
using Microsoft.Extensions.Logging;

namespace Desyco.Dms.Infrastructure.Seeders;

public class ApplicationDbSeeder(IEnumerable<IDataSeeder> seeders, ILogger<ApplicationDbSeeder> logger)
{
    public async Task SeedAsync()
    {
        logger.LogSeedingStarted();
        foreach (var seeder in seeders)
        {
            try
            {
                logger.LogSeedingExecution(seeder.GetType().Name);
                await seeder.SeedAsync();
            }
            catch (Exception ex)
            {
                logger.LogSeedingError(ex, seeder.GetType().Name);
                throw;
            }
        }
        logger.LogSeedingCompleted();
    }
}
