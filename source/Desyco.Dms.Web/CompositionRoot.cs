using System.IdentityModel.Tokens.Jwt;
using Desyco.Dms.Domain;
using Desyco.Dms.Domain.Common;
using Desyco.Dms.Infrastructure;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Desyco.Dms.Web;

public static class CompositionRoot
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.ConfigureDomainServices(configuration, environment);
        // services.ConfigureApplicationServices(configuration, environment);
        services.ConfigureInfrastructureServices(configuration, environment);
        services.ConfigureHangfireServices(configuration);
        // services.AddWebHandlersFromAssembly(typeof(AppComposition).Assembly);
        services.ConfigureAuthServices(configuration);

        return services;
    }

    public static void ExecuteMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var migrationExecutor = scope.ServiceProvider.GetRequiredService<IMigrationExecutor>();

        migrationExecutor.Migrate();
    }

    private static void ConfigureAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var authenticationBuilder = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

        authenticationBuilder.AddJwtBearer(o =>
            {
                o.Authority = configuration["Authorization:Authority"];
                o.Audience = configuration["Authorization:Audience"];
            }
        );
    }

    private static void ConfigureHangfireServices(this IServiceCollection services, IConfiguration configuration)
    {
        var hangfireConnectionString = configuration.GetConnectionString(ConnectionStringName.Hangfire);

        services.AddHangfire(c =>
        {
            c.UseSqlServerStorage(hangfireConnectionString);
            c.UseSerilogLogProvider();
        });
    }
}
