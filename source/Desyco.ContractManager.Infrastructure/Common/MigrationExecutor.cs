using Desyco.ContractManager.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Desyco.ContractManager.Infrastructure.Common;

[ExcludeFromCodeCoverage]
public class MigrationExecutor(ApplicationDbContext context) : IMigrationExecutor
{
    public void Migrate() => context.Database.Migrate();
}
