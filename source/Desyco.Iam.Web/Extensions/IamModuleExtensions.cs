using System.Text;
using Desyco.Iam.Contracts.Interfaces;
using Desyco.Iam.Infrastructure.Authentication.Permissions; // Added this line
using Desyco.Iam.Infrastructure.Authentication.Services;
using Desyco.Iam.Infrastructure.Authentication.Settings;
using Desyco.Iam.Infrastructure.Persistence.Context;
using Desyco.Iam.Infrastructure.Persistence.Entities;
using Desyco.Iam.Infrastructure.Seeders; // Added this line
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Desyco.Iam.Web.Extensions;

public static class IamModuleExtensions
{
    public static IServiceCollection AddIamModule(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Configure JwtSettings
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        // 2. Configure DbContext
        services.AddDbContext<IamDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("IamConnection"),
                b => b.MigrationsAssembly(typeof(IamDbContext).Assembly.FullName)));

        // 3. Configure Identity
        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<IamDbContext>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddDefaultTokenProviders();

        // 4. Configure Authentication & JWT
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
            };
        });

        // 5. Register Services
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<FeatureSeeder>();
        services.AddScoped<IPermissionService, PermissionService>(); // Register PermissionService

        return services;
    }
}
