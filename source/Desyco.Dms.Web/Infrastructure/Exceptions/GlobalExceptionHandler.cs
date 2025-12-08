using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Desyco.Dms.Application.Common.Exceptions;

namespace Desyco.Dms.Web.Infrastructure.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Title = "An error occurred",
            Detail = "An unexpected error occurred while processing your request."
        };

        switch (exception)
        {
            case ValidationException validationException:
                problemDetails.Status = (int)HttpStatusCode.BadRequest;
                problemDetails.Title = "Validation Error";
                problemDetails.Detail = "One or more validation errors occurred.";
                problemDetails.Extensions["errors"] = validationException.Errors;
                break;
            // TODO: Add more specific exception handling if needed (e.g., NotFoundException, UnauthorizedException)
            default:
                break;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
