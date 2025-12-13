namespace Desyco.Dms.Application.Common.Interfaces;

public interface ILocalizationService
{
    /// <summary>
    /// Returns a queryable string that can be used within an EF Core projection (Select)
    /// to fetch the translated value for a given key and language code.
    /// This translates to a subquery in SQL.
    /// </summary>
    /// <param name="key">The translation key.</param>
    /// <param name="languageCode">The language code (e.g., "en-US").</param>
    /// <returns>An IQueryable of string representing the translated value.</returns>
    IQueryable<string> GetTranslationQuery(string key, string languageCode);
}
