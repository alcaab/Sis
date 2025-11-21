using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Desyco.ContractManager.Domain;

public static class DomainComposition
{
    public static IServiceCollection ConfigureDomainServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        => services;
}
