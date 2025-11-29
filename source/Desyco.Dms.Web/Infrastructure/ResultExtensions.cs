// using Desyco.Dms.Application.Common.Results;
// using Desyco.Dms.Application.Common.Results.Error;
// using Desyco.Dms.Application.Common.Results.ResultModels;
// using Microsoft.AspNetCore.Mvc;
//
// namespace Desyco.Dms.Web.Infrastructure;
//
// public static class ResultExtensions
// {
//     public static ActionResult ToActionResult(this Result result)
//     {
//         return result.IsSuccess ? new NoContentResult() : result.Error.ToActionResult();
//     }
//
//     public static ActionResult<T> ToActionResult<T>(this Result<T> result)
//     {
//         if (result.IsSuccess)
//         {
//             return result switch
//             {
//                 CreatedResult<T> createdResult => new CreatedResult(createdResult.Location, createdResult.Value),
//                 null => new NoContentResult(),
//                 _ when result.Value is not null => new OkObjectResult(result.Value),
//                 _ => new NoContentResult()
//             };
//         }
//
//         return result.Error.ToActionResult<T>();
//     }
//
//     public static async Task<ActionResult<T>> ToActionResult<T>(this Task<Result<T>> resultTask)
//     {
//         var result = await resultTask;
//         return result.ToActionResult();
//     }
//
//     private static ActionResult<T> ToActionResult<T>(this Error error)
//     {
//         return error.ToActionResult();
//     }
//
//     private static ActionResult ToActionResult(this Error error)
//     {
//         var details = new ProblemDetails
//         {
//             Type = $"https://netto-online.de/developers/docs/problems/{error.Type.ToString().ToLowerInvariant()}",
//             Title = error.Type.GetTitle(),
//             StatusId = error.Type.GetStatusCode(),
//             Detail = error.Message
//         };
//
//         if (error.Metadata is not { Count: > 0 })
//         {
//             return new ObjectResult(details) { StatusCode = details.StatusId };
//         }
//
//         foreach (var kvp in error.Metadata)
//             details.Extensions[kvp.Key] = kvp.Value;
//
//         return new ObjectResult(details)
//         {
//             StatusCode = details.StatusId
//         };
//     }
//
//     private static string GetTitle(this ErrorType type) => type switch
//     {
//         ErrorType.Validation => "Validation error",
//         ErrorType.Conflict => "Conflict",
//         ErrorType.NotFound => "Not Found",
//         ErrorType.Forbidden => "Forbidden",
//         ErrorType.DependencyNotFound => "Dependency not found",
//         ErrorType.Unexpected => "Server error",
//         _ => "Request error"
//     };
//
//     private static int GetStatusCode(this ErrorType type) => type switch
//     {
//         ErrorType.Validation => StatusCodes.Status400BadRequest,
//         ErrorType.Conflict => StatusCodes.Status409Conflict,
//         ErrorType.NotFound => StatusCodes.Status404NotFound,
//         ErrorType.Forbidden => StatusCodes.Status403Forbidden,
//         ErrorType.DependencyNotFound => StatusCodes.Status400BadRequest,
//         ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
//         _ => StatusCodes.Status400BadRequest
//     };
// }
