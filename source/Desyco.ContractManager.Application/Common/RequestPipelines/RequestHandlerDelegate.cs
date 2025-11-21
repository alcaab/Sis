namespace Desyco.ContractManager.Application.Common.RequestPipelines;

public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();
