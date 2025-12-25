using Desyco.Iam.Contracts.Permissions;
using Desyco.Iam.Infrastructure.Persistence.Context;
using Desyco.Iam.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Desyco.Iam.Infrastructure.Seeders;

public class FeatureSeeder(IamDbContext context)
{
    public async Task SeedAsync()
    {
        var features = new List<Feature>
        {
            new()
            {
                Code = Permissions.AcademicYears.Code,
                Description = "feature.academicYears",
                Group = "Academic",
                Order = 100
            },
            new()
            {
                Code = Permissions.EvaluationPeriods.Code,
                Description = "feature.evaluationPeriods",
                Group = "Academic",
                Order = 150
            },
            new()
            {
                Code = Permissions.Students.Code,
                Description = "feature.students",
                Group = "Academic",
                Order = 200,
                CustomPermissions = "Export"
            },
            new()
            {
                Code = Permissions.Teachers.Code,
                Description = "feature.teachers",
                Group = "Academic",
                Order = 300
            },
            new()
            {
                Code = Permissions.Classrooms.Code,
                Description = "feature.classrooms",
                Group = "Academic",
                Order = 400
            },
            new()
            {
                Code = Permissions.Enrollments.Code,
                Description = "feature.enrollments",
                Group = "Academic",
                Order = 500,
                CustomPermissions = "Approve:Approve Enrollment,Reject:Reject Enrollment"
            },
            new()
            {
                Code = Permissions.Security.Code,
                Description = "feature.permissions",
                Group = "Administration",
                Order = 1000
            },
            new()
            {
                Code = Permissions.Users.Code,
                Description = "feature.users",
                Group = "Administration",
                Order = 1100
            },
            new()
            {
                Code = Permissions.Roles.Code,
                Description = "feature.roles",
                Group = "Administration",
                Order = 1200
            }
        };

        foreach (var feature in features)
        {
            var existing = await context.Features.FirstOrDefaultAsync(f => f.Code == feature.Code);
            if (existing == null)
            {
                feature.Id = Guid.NewGuid();
                await context.Features.AddAsync(feature);
            }
            else
            {
                // Update existing
                existing.Description = feature.Description;
                existing.Group = feature.Group;
                existing.Order = feature.Order;
                existing.CustomPermissions = feature.CustomPermissions;
            }
        }

        await context.SaveChangesAsync();
    }
}
