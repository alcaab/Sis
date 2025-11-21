using Microsoft.Extensions.Logging;

namespace Desyco.Dms.Application.Common.RequestPipelines.Behaviors;

public static partial class BehaviorLog
{
    [LoggerMessage(
        EventId = ApplicationLogEventIds.Pipelines.NoValidatorsFound,
        Level = LogLevel.Debug,
        Message = "No validators found for request of type {RequestType}.")]
    public static partial void NoValidatorsFound(this ILogger logger, string requestType);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.Pipelines.ValidationFailed,
        Level = LogLevel.Information,
        Message = "Validation failed for request of type {RequestType}. It has {FailuresCount} failures.")]
    public static partial void ValidationFailed(this ILogger logger, string requestType, int failuresCount);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.Pipelines.ValidationPassed,
        Level = LogLevel.Debug,
        Message = "Validation passed for request of type {RequestType}.")]
    public static partial void ValidationPassed(this ILogger logger, string requestType);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.Pipelines.ExecutingValidators,
        Level = LogLevel.Debug,
        Message = "Executing {ValidatorsCount} validators for request of type {RequestType}.")]
    public static partial void ExecutingValidators(this ILogger logger, string requestType, int validatorsCount);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.Pipelines.CheckingPermissionsForRequest,
        Level = LogLevel.Debug,
        Message = "Checking permissions for request of type {RequestType}.")]
    public static partial void CheckingPermissionsForRequest(this ILogger logger, string requestType);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.Pipelines.CheckingUserAuthentication,
        Level = LogLevel.Debug,
        Message = "User {Subject} authenticated as Client: {IsClient}.")]
    public static partial void UserAuthenticatedAs(this ILogger logger, string? subject, bool isClient);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.Pipelines.UserIsNotAuthenticated,
        Level = LogLevel.Information,
        Message = "User is not authenticated.")]
    public static partial void UserIsNotAuthenticated(this ILogger logger);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.Pipelines.UserDoesNotHavePermission,
        Level = LogLevel.Warning,
        Message = "User {Subject} does not have permission '{Permission}'.")]
    public static partial void UserDoesNotHavePermission(this ILogger logger, string? subject, string permission);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.Pipelines.UserDoesNotHavePermissions,
        Level = LogLevel.Warning,
        Message = "User {Subject} does not have any of the required permissions.")]
    public static partial void UserDoesNotHavePermissions(this ILogger logger, string? subject);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.Pipelines.UserDoesNotHavePermissions,
        Level = LogLevel.Warning,
        Message = "User {Subject} does not have any of the required permissions. Reason: {Message}")]
    public static partial void UserDoesNotHavePermissions(this ILogger logger, string? subject, string? message);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.Pipelines.NoAuthGuardsFound,
        Level = LogLevel.Debug,
        Message = "No standlone auth guards found for request of type {RequestType}.")]
    public static partial void NoAuthGuardsFound(this ILogger logger, string requestType);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.Pipelines.AuthorizationSuccess,
        Level = LogLevel.Debug,
        Message = "Authorization succeeded for request of type {RequestType} by user {Subject}.")]
    public static partial void AuthorizationSuccess(this ILogger logger, string requestType, string? subject);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.Pipelines.UnhandledExceptionWhileProcessingRequest,
        Level = LogLevel.Error,
        Message = "An unhandled exception occured while processing the request.")]
    public static partial void UnhandledExceptionWhileProcessingRequest(this ILogger logger, Exception exception);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.Pipelines.JsonDeserializationError,
        Level = LogLevel.Error,
        Message = "An error occurred while deserializing the request body")]
    public static partial void JsonDeserializationError(this ILogger logger, Exception exception);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.DataSync.BulkCopyError,
        Level = LogLevel.Error,
        Message = "An error occurred while bulk copying data")]
    public static partial void BulkCopyError(this ILogger logger, Exception exception);
}
