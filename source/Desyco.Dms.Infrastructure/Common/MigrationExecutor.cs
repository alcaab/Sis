using Desyco.Dms.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Desyco.Dms.Infrastructure.Common;

[ExcludeFromCodeCoverage]
public class MigrationExecutor(ApplicationDbContext context) : IMigrationExecutor
{
    public void Migrate() => context.Database.Migrate();
}
