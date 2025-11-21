namespace Desyco.Dms.Application.Common.RequestPipelines;

public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();
