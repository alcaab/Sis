using Desyco.Iam.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;

namespace Desyco.Iam.Infrastructure.Seeders;

public class SecuritySeeder(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
{
    public async Task SeedAsync()
    {
        // 1. Seed Roles (Based on GibbonEdu standard roles)
        var roles = new[] { "Admin", "Teacher", "Student", "Parent", "SupportStaff" };
        
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new ApplicationRole(roleName) 
                { 
                    Description = $"System default role for {roleName}" 
                });
            }
        }

        // 2. Seed Test Users
        const string password = "Password123!"; // Common password for all test users
        
        var users = new (string Email, string Role)[]
        {
            ("admin@school.com", "Admin"),
            ("teacher@school.com", "Teacher"),
            ("student@school.com", "Student"),
            ("parent@school.com", "Parent"),
            ("support@school.com", "SupportStaff")
        };

        foreach (var (email, role) in users)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = role, // Use Role as FirstName for easy ID
                    LastName = "TestUser",
                    EmailConfirmed = true
                };
                
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
