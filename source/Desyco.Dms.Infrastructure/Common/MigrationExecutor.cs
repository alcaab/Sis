using Desyco.Dms.Domain.Common;

namespace Desyco.Dms.Infrastructure.Common;

[ExcludeFromCodeCoverage]
public class MigrationExecutor(ApplicationDbContext context) : IMigrationExecutor
{
    public void Migrate() => context.Database.Migrate();
}
