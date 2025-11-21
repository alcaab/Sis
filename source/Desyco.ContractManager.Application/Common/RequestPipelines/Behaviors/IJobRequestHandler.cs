namespace Desyco.ContractManager.Application.Common.RequestPipelines.Behaviors;

public interface IJobRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
