using Desyco.ContractManager.Application.Common.RequestPipelines;
using Desyco.ContractManager.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace Desyco.ContractManager.Web.Infrastructure;

public static class RequestExecutorExtensions
{
    public static async Task<ActionResult<TResult>> Handle<TResult>(
        this IRequestExecutor executor,
        IRequest<Result<TResult>> request,
        CancellationToken cancellationToken = default)
    {
        var result = await executor.ExecuteAsync(request, cancellationToken);
        return result.ToActionResult();
    }

    public static async Task<ActionResult> Handle(
        this IRequestExecutor executor,
        IRequest<Result> request,
        CancellationToken cancellationToken = default)
    {
        var result = await executor.ExecuteAsync(request, cancellationToken);
        return result.ToActionResult();
    }
}
