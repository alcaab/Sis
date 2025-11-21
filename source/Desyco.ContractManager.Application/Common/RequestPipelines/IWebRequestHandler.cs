namespace Desyco.ContractManager.Application.Common.RequestPipelines;

public interface IWebRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
