namespace Desyco.ContractManager.Application.Common.RequestPipelines;

public interface IRequestExecutor
{
    Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken);
}
