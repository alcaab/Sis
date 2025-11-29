using Autofac;
using Desyco.Dms.Infrastructure.Common.Seeding;
using Desyco.Dms.Infrastructure.Seeders;

namespace Desyco.Dms.Infrastructure;

public class InfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // "Repository"
        builder.RegisterAssemblyTypes(typeof(InfrastructureModule).Assembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        // Seeders
        builder.RegisterAssemblyTypes(typeof(InfrastructureModule).Assembly)
            .AssignableTo<IDataSeeder>()
            .As<IDataSeeder>()
            .InstancePerLifetimeScope();

        builder.RegisterType<ApplicationDbSeeder>()
            .AsSelf()
            .InstancePerLifetimeScope();
    }
}
