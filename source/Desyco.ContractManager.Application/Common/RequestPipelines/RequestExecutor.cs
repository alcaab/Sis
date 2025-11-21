using Desyco.ContractManager.Application.Common.RequestPipelines.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Desyco.ContractManager.Application.Common.RequestPipelines;

public class RequestExecutor(IServiceProvider serviceProvider) : IRequestExecutor
{
    public Task<TResponse> ExecuteAsync<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var responseType = typeof(TResponse);

        Type? handlerType = null;

        var possibleHandlerTypes = new[]
        {
            typeof(IRequestHandler<,>),
            typeof(IJobRequestHandler<,>),
            typeof(IWebRequestHandler<,>)
        };

        foreach (var baseHandler in possibleHandlerTypes)
        {
            var possibleType = baseHandler.MakeGenericType(requestType, responseType);

            var handler = serviceProvider.GetService(possibleType);

            if (handler is null)
            {
                continue;
            }

            handlerType = possibleType;
            break;
        }

        if (handlerType is null)
        {
            throw new InvalidOperationException();
        }

        var handlerInstance = serviceProvider.GetRequiredService(handlerType);

        var pipelineType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType);
        var behaviors = serviceProvider.GetServices(pipelineType);

        RequestHandlerDelegate<TResponse> next = () =>
            ((dynamic)handlerInstance).HandleAsync((dynamic)request, cancellationToken);

        foreach (var behavior in behaviors.Cast<dynamic>().Reverse())
        {
            var currentNext = next;
            next = () => behavior.HandleAsync((dynamic)request, currentNext, cancellationToken);
        }

        return next();
    }
}
