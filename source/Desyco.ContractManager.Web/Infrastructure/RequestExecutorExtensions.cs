using Desyco.Dms.Application.Common.RequestPipelines;
using Desyco.Dms.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace Desyco.Dms.Web.Infrastructure;

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
