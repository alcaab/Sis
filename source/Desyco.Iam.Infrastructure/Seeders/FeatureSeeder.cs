using Desyco.Iam.Contracts.Permissions;
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
                new Feature { Id = Guid.NewGuid(), Code = Permissions.AcademicYears.Code, Description = "feature.academicyears", Group = "Academic", Order = 100 },
                new Feature { Id = Guid.NewGuid(), Code = Permissions.Students.Code, Description = "feature.students", Group = "Academic", Order = 200 },
                new Feature { Id = Guid.NewGuid(), Code = Permissions.Teachers.Code, Description = "feature.teachers", Group = "Academic", Order = 300 },
                new Feature { Id = Guid.NewGuid(), Code = Permissions.Classrooms.Code, Description = "feature.classrooms", Group = "Academic", Order = 400 },
                new Feature { Id = Guid.NewGuid(), Code = Permissions.Enrollments.Code, Description = "feature.enrollments", Group = "Academic", Order = 500 },
                new Feature { Id = Guid.NewGuid(), Code = Permissions.Security.Code, Description = "feature.permissions", Group = "Administration", Order = 1000 },
                new Feature { Id = Guid.NewGuid(), Code = Permissions.Users.Code, Description = "feature.users", Group = "Administration", Order = 1100 },
                new Feature { Id = Guid.NewGuid(), Code = Permissions.Roles.Code, Description = "feature.roles", Group = "Administration", Order = 1200 }
            };

            await context.Features.AddRangeAsync(features);
            await context.SaveChangesAsync();
        }
    }
}
