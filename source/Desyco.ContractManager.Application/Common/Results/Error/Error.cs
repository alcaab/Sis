using System.ComponentModel.DataAnnotations;

namespace Desyco.Dms.Application.Common.Results.Error;

public record Error(ErrorType Type, string Message)
{
    public static Error Failure() =>
        new(ErrorType.Failure, "The request has failed.");

    public static Error Unexpected(Exception ex) =>
        new(ErrorType.Unexpected, "An unexpected error has occurred.")
        {
            Metadata = new Dictionary<string, object> { { "exception", ex } }
        };

    public static Error NotFound() =>
        new(ErrorType.NotFound, "The requested item was not found.");

    public static Error Forbidden() =>
        new(ErrorType.Forbidden, "Access to this resource denied.");

    public static Error Unauthorized() =>
        new(ErrorType.Unauthorized, "You are not authorized. Please sign in.");

    public static Error Validation(IEnumerable<ValidationResult> validationErrors)
    {
        var metaData = validationErrors
            .SelectMany(err => err.MemberNames.Select(member => new { member, err.ErrorMessage }))
            .ToDictionary(
                x => x.member, object (x) => x.ErrorMessage ?? "Validation error",
                StringComparer.OrdinalIgnoreCase
            );

        return new Error(ErrorType.Validation, "The request was not processed due to validation errors.")
        {
            Metadata = new Dictionary<string, object> { { "validationErrors", metaData } }
        };
    }

    public static Error Conflict(string identifier) =>
        new(ErrorType.Conflict, "An item with the same key already exists.")
        {
            Metadata = new Dictionary<string, object> { { "conflictingIdentifier", identifier } }
        };

    public static Error DependencyNotFound(string identifier) =>
        new(ErrorType.DependencyNotFound, "A necessary dependency was not found.")
        {
            Metadata = new Dictionary<string, object> { { "missingIdentifier", identifier } }
        };

    public Dictionary<string, object>? Metadata { get; init; }
}
