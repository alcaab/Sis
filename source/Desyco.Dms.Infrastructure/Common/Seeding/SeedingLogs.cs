using Desyco.Dms.Application;
using Microsoft.Extensions.Logging;

namespace Desyco.Dms.Infrastructure.Common.Seeding;

internal static partial class SeedingLogs
{
    // ApplicationDbSeeder
    [LoggerMessage(EventId = ApplicationLogEventIds.Seeding.SeedingStarted, Level = LogLevel.Information, Message = "Starting database seeding...")]
    public static partial void LogSeedingStarted(this ILogger logger);

    [LoggerMessage(EventId = ApplicationLogEventIds.Seeding.SeedingCompleted, Level = LogLevel.Information, Message = "Database seeding completed.")]
    public static partial void LogSeedingCompleted(this ILogger logger);

    [LoggerMessage(EventId = ApplicationLogEventIds.Seeding.SeedingExecution, Level = LogLevel.Information, Message = "Executing seeder: {SeederName}")]
    public static partial void LogSeedingExecution(this ILogger logger, string seederName);

    [LoggerMessage(EventId = ApplicationLogEventIds.Seeding.SeedingError, Level = LogLevel.Error, Message = "Error executing seeder {SeederName}")]
    public static partial void LogSeedingError(this ILogger logger, Exception ex, string seederName);

    // EnumEntitySeeder
    [LoggerMessage(EventId = ApplicationLogEventIds.Seeding.EnumEntityDeleted, Level = LogLevel.Information, Message = "Deleted obsolete {EntityName} with Id {EntityId}")]
    public static partial void LogEntityDeleted(this ILogger logger, string entityName, object entityId);

    [LoggerMessage(EventId = ApplicationLogEventIds.Seeding.EnumEntityDeleteSkipped, Level = LogLevel.Warning, Message = "Could not delete {EntityName} with Id {EntityId} because it is in use.")]
    public static partial void LogEntityDeleteSkippedInUse(this ILogger logger, string entityName, object entityId);

    [LoggerMessage(EventId = ApplicationLogEventIds.Seeding.EnumEntityDeleteError, Level = LogLevel.Error, Message = "Error deleting {EntityName} with Id {EntityId}")]
    public static partial void LogEntityDeleteError(this ILogger logger, Exception ex, string entityName, object entityId);
}
