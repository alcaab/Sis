using Autofac;
using Desyco.Dms.Application.Common.Behaviours;
using FluentValidation;
using Desyco.Mediator;

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

        // Register all FluentValidation Validators found in the assembly
        builder.RegisterAssemblyTypes(typeof(ApplicationModule).Assembly)
            .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        // Register ValidationBehaviour in the MediatR pipeline
        builder.RegisterGeneric(typeof(ValidationBehaviour<,>))
            .As(typeof(IPipelineBehavior<,>))
            .InstancePerLifetimeScope();
    }
}
