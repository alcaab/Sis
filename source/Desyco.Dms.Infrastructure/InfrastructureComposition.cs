using Desyco.Dms.Application.Common;
using Desyco.Dms.Domain.AcademicYears.Interfaces;
using Desyco.Dms.Domain.Common;
using Desyco.Dms.Infrastructure.AcademicYears.Repositories;
using Desyco.Dms.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Desyco.Dms.Infrastructure;

public static class InfrastructureComposition
{
    public static void ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.ConfigureDatabaseServices(configuration);
        services.AddSingleton(TimeProvider.System);

        services.AddScoped<IAcademicYearRepository, AcademicYearRepository>();
    }

    private static void ConfigureDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(ConnectionStringName.Application),
                contextOptionsBuilder =>
                {
                    contextOptionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    contextOptionsBuilder.EnableRetryOnFailure();
                }));

        services.AddScoped<IUnitOfWork>(c => c.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IMigrationExecutor, MigrationExecutor>();
    }
}
