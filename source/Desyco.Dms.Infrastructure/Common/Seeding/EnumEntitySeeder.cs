using Desyco.Dms.Domain.Common.Base;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Desyco.Dms.Infrastructure.Common.Seeding;

public abstract partial class EnumEntitySeeder<TEntity, TEnum>(ApplicationDbContext context, ILogger logger) : IDataSeeder
    where TEntity : EntityBase<TEnum>
    where TEnum : struct, Enum
{
    protected abstract TEntity CreateEntity(TEnum id);

    public async Task SeedAsync()
    {
        var enumValues = Enum.GetValues<TEnum>();
        var dbSet = context.Set<TEntity>();
        var existingEntities = await dbSet.ToDictionaryAsync(e => e.Id);
        
        var prefix = typeof(TEnum).Name.ToUpperInvariant();
        if (prefix.EndsWith("TYPE"))
        {
            prefix = prefix[..^4];
        }

        // 1. Handle Updates and Inserts
        foreach (var enumValue in enumValues)
        {
            var rawName = enumValue.ToString();
            var name = HumanizePascalCase(rawName);
            var translationKey = $"ENUM_{prefix}_{rawName.ToUpperInvariant()}";
            
            if (existingEntities.TryGetValue(enumValue, out var entity))
            {
                // Update
                // if (entity.TranslationKey != translationKey || GetName(entity) != name)
                // {
                //     entity.TranslationKey = translationKey;
                //     SetName(entity, name);
                // }
                existingEntities.Remove(enumValue);
            }
            else
            {
                // Insert
                var newEntity = CreateEntity(enumValue);
                // newEntity.TranslationKey = translationKey;
                SetName(newEntity, name);
                await dbSet.AddAsync(newEntity);
            }
        }
        
        // Save Adds/Updates first
        await context.SaveChangesAsync();

        // 2. Handle Deletes (Remaining items in existingEntities)
        foreach (var entityToDelete in existingEntities.Values)
        {
            try
            {
                dbSet.Remove(entityToDelete);
                await context.SaveChangesAsync();
                logger.LogEntityDeleted(typeof(TEntity).Name, entityToDelete.Id);
            }
            catch (DbUpdateException)
            {
                // Detach the entity so it doesn't block future saves
                context.Entry(entityToDelete).State = EntityState.Detached; 
                logger.LogEntityDeleteSkippedInUse(typeof(TEntity).Name, entityToDelete.Id);
            }
            catch (Exception ex)
            {
                 context.Entry(entityToDelete).State = EntityState.Detached;
                 logger.LogEntityDeleteError(ex, typeof(TEntity).Name, entityToDelete.Id);
            }
        }
    }

    private static void SetName(TEntity entity, string name)
    {
        var prop = typeof(TEntity).GetProperty("Name");
        if (prop != null && prop.CanWrite)
        {
            prop.SetValue(entity, name);
        }
    }
    
    private static string GetName(TEntity entity)
    {
        var prop = typeof(TEntity).GetProperty("Name");
        return prop?.GetValue(entity)?.ToString() ?? string.Empty;
    }

    [GeneratedRegex("((?<!^)[A-Z](?=[a-z]))|((?<=[a-z])(?=[A-Z]))")]
    private static partial Regex PascalCaseRegex();

    // Custom PascalCase Humanizer
    private static string HumanizePascalCase(string pascalCaseString)
    {
        if (string.IsNullOrWhiteSpace(pascalCaseString))
        {
            return string.Empty;
        }

        // Add space before each capital letter that is not the first character
        // and is not preceded by another capital letter (to handle acronyms like "USA")
        var result = PascalCaseRegex().Replace(pascalCaseString, " $1$2").Trim();
        
        // Capitalize the first letter if it's not already
        if (result.Length > 0 && char.IsLower(result[0]))
        {
            result = char.ToUpper(result[0]) + result[1..];
        }

        return result;
    }
}
