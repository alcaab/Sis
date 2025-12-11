using Desyco.Dms.Application;
using Desyco.Dms.Domain;
using Desyco.Dms.Domain.Common;
using Desyco.Dms.Infrastructure;
using Desyco.Dms.Web.Infrastructure;
using Desyco.Iam.Infrastructure.Persistence.Context;
using Desyco.Iam.Web.Extensions;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Scrima.OData;
using Scrima.OData.AspNetCore;

namespace Desyco.Dms.Web;

public static class CompositionRoot
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.ConfigureDomainServices(configuration, environment);
        services.ConfigureApplicationServices(configuration, environment);
        services.ConfigureInfrastructureServices(configuration, environment);
        services.ConfigureHangfireServices(configuration);
        // services.AddWebHandlersFromAssembly(typeof(AppComposition).Assembly);
        services.AddIamModule(configuration);
        services.ConfigureODataQuery();

        return services;
    }

    public static void ExecuteMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var migrationExecutor = scope.ServiceProvider.GetRequiredService<IMigrationExecutor>();

        migrationExecutor.Migrate();

        var iamContext = scope.ServiceProvider.GetRequiredService<IamDbContext>();
        iamContext.Database.Migrate();
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
    
    public static void ConfigureODataQuery(this IServiceCollection services)
    {
        services.AddODataQuery();

        services.Replace(new ServiceDescriptor(
            typeof(IODataRawQueryParser),
            typeof(ODataSortRawQueryParser),
            ServiceLifetime.Singleton));
    }
}
