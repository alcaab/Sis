using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Desyco.Dms.Web.Infrastructure.Middleware;

public class ProblemDetailsMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        // Check if response is 401 or 403 and has no body content yet
        // We only want to intervene if the response body is empty (meaning the framework didn't write a custom response)
        if ((context.Response.StatusCode == StatusCodes.Status401Unauthorized ||
             context.Response.StatusCode == StatusCodes.Status403Forbidden) &&
            !context.Response.HasStarted)
        {
            var problemDetails = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Instance = context.Request.Path
            };

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                problemDetails.Title = "Unauthorized";
                problemDetails.Detail = "Authentication is required to access this resource.";
                problemDetails.Type = "https://tools.ietf.org/html/rfc7235#section-3.1";
            }
            else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                problemDetails.Title = "Forbidden";
                problemDetails.Detail = "You do not have permission to perform this action.";
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3";
            }

            context.Response.ContentType = "application/problem+json";
            
            // Serialize and write
            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }
}
