using Autofac;

namespace Desyco.Dms.Infrastructure;

public class InfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // Escanea y registra todos los tipos que terminen en "Repository"
        // y los asigna a sus interfaces implementadas (ej. AcademicYearRepository -> IAcademicYearRepository)
        builder.RegisterAssemblyTypes(typeof(InfrastructureModule).Assembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        // Aquí puedes añadir otros servicios de infraestructura si es necesario
    }
}
