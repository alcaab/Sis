using Autofac;

namespace Desyco.Dms.Application;

public class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // Registra todos los Mappers que terminen en "Mapper"
        builder.RegisterAssemblyTypes(typeof(ApplicationModule).Assembly)
            .Where(t => t.Name.EndsWith("Mapper"))
            .AsSelf()
            .SingleInstance();
            
        // Aquí puedes añadir más registros específicos de Application
    }
}
