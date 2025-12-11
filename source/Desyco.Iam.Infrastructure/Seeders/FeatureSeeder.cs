using Desyco.Iam.Infrastructure.Persistence.Context;
using Desyco.Iam.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Desyco.Iam.Infrastructure.Seeders;

public class FeatureSeeder(IamDbContext context)
{
    public async Task SeedAsync()
    {
        if (!await context.Features.AnyAsync())
        {
            var features = new List<Feature>
            {
                new Feature { Id = Guid.NewGuid(), Code = "AcademicYears", Description = "feature.academicyears", Group = "Academic", Order = 100 },
                new Feature { Id = Guid.NewGuid(), Code = "Students", Description = "feature.students", Group = "Academic", Order = 200 },
                new Feature { Id = Guid.NewGuid(), Code = "Teachers", Description = "feature.teachers", Group = "Academic", Order = 300 },
                new Feature { Id = Guid.NewGuid(), Code = "Classrooms", Description = "feature.classrooms", Group = "Academic", Order = 400 },
                new Feature { Id = Guid.NewGuid(), Code = "Enrollments", Description = "feature.enrollments", Group = "Academic", Order = 500 },
                new Feature { Id = Guid.NewGuid(), Code = "Permissions", Description = "feature.permissions", Group = "Administration", Order = 1000 },
                new Feature { Id = Guid.NewGuid(), Code = "Users", Description = "feature.users", Group = "Administration", Order = 1100 },
                new Feature { Id = Guid.NewGuid(), Code = "Roles", Description = "feature.roles", Group = "Administration", Order = 1200 }
            };

            await context.Features.AddRangeAsync(features);
            await context.SaveChangesAsync();
        }
    }
}
