using Desyco.Dms.Domain.Students;
using Desyco.Dms.Infrastructure.Common.Seeding;
using Microsoft.Extensions.Logging;

namespace Desyco.Dms.Infrastructure.Seeders;

public class StudentStatusSeeder(ApplicationDbContext context, ILogger<StudentStatusSeeder> logger) 
    : EnumEntitySeeder<StudentStatusEntity, StudentStatus>(context, logger)
{
    protected override StudentStatusEntity CreateEntity(StudentStatus id) => new() { Id = id };
}
