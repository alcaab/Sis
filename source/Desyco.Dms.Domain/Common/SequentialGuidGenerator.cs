using System.Security.Cryptography;

namespace Desyco.Dms.Domain.Common;

/// <summary>
/// Generates sequential GUIDs (also known as COMB GUIDs) that are more performant
/// for use as primary keys in SQL Server clustered indexes than random GUIDs.
/// This implementation produces GUIDs that sort by the last 6 bytes, allowing
/// for sequential storage without fragmenting database indexes.
/// </summary>
public static class SequentialGuidGenerator
{
    private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

    /// <summary>
    /// Generates a new sequential GUID suitable for SQL Server (combining timestamp with random bytes).
    /// </summary>
    /// <returns>A new sequential GUID.</returns>
    public static Guid NewGuid()
    {
        var guidBytes = new byte[16];
        _rng.GetBytes(guidBytes);

        var timestamp = DateTime.UtcNow.Ticks;

        var timestampBytes = BitConverter.GetBytes(timestamp);

        // Reverse the timestamp bytes if on a little-endian system to ensure proper sorting in SQL Server
        // (SQL Server's uniqueidentifier type stores GUIDs in a mixed-endian format internally,
        // but sorting is based on the last 6 bytes which are typically timestamp-derived).
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(timestampBytes);
        }

        // Overwrite the last 6 bytes of the GUID with timestamp bytes
        // This makes the GUIDs sequential based on creation time, which is good for clustered indexes.
        Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);

        return new Guid(guidBytes);
    }
}
