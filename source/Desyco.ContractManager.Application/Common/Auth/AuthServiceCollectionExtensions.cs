using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Desyco.ContractManager.Application.Common.Auth;

public static class AuthServiceCollectionExtensions
{
    public static IServiceCollection AddAuthGuardsFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var authGuardInterface = typeof(IAuthGuard<>);
        var authGuards = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == authGuardInterface))
            .ToList();

        foreach (var authGuard in authGuards)
        {
            var implementedInterface = authGuard.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == authGuardInterface);
            services.AddScoped(implementedInterface, authGuard);
        }

        return services;
    }
}
