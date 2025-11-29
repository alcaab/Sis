using System.Reflection;
using Desyco.Dms.Domain.Common;
using Desyco.Dms.Domain.Common.Attributes;
using Scrima.Core.Query;
using Scrima.OData;

namespace Desyco.Dms.Web.Infrastructure;

/// <summary>
/// Custom parser for OData raw queries.
/// </summary>
public class ODataSortRawQueryParser : ODataRawRawQueryParser, IODataRawQueryParser
{
    /// <summary>
    /// Parses the OData raw query options.
    /// </summary>
    /// <param name="itemType">The type of the item.</param>
    /// <param name="rawQuery">The raw query options.</param>
    /// <param name="defaultOptions">The default query options.</param>
    /// <returns>The parsed query options.</returns>
    public new QueryOptions ParseOptions(Type itemType, ODataRawQueryOptions rawQuery, ODataQueryDefaultOptions? defaultOptions = null)
    {
        if (rawQuery.OrderBy is null)
        {
            SetDefaultOrderBy(itemType, rawQuery);
        }

        var result = base.ParseOptions(itemType, rawQuery, defaultOptions);

        return result;
    }

    /// <summary>
    /// Sets the default order by clause for the query options.
    /// </summary>
    /// <param name="itemType">The type of the item.</param>
    /// <param name="queryOptions">The query options.</param>
    private static void SetDefaultOrderBy(Type itemType, ODataRawQueryOptions queryOptions)
    {
        var defaultOrderBy = string.Join(",", itemType.GetProperties()
            .Where(prop => prop.IsDefined(typeof(OrderByPropAttribute), true))
            .Select(prop =>
            {
                var attribute = prop.GetCustomAttribute<OrderByPropAttribute>();
                var name = attribute?.Name ?? prop.Name;
                return attribute?.Direction == SortDirection.Asc ? name : $"{name} desc";
            }));

        if (string.IsNullOrWhiteSpace(defaultOrderBy))
            return;

        queryOptions.OrderBy = defaultOrderBy;
    }
}
