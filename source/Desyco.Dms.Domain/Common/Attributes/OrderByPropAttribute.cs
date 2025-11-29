namespace Desyco.Dms.Domain.Common.Attributes;

/// <summary>
/// Attribute to specify the order by property for sorting.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class OrderByPropAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderByPropAttribute"/> class with a default index of 0.
    /// </summary>
    public OrderByPropAttribute() : this(0)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderByPropAttribute"/> class with the specified index.
    /// </summary>
    /// <param name="index">The index of the property in the order by clause.</param>
    public OrderByPropAttribute(int index)
    {
        Index = index;
    }

    /// <summary>
    /// Gets or sets the name of the property.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the sort direction.
    /// </summary>
    public SortDirection Direction { get; set; } = SortDirection.Asc;

    /// <summary>
    /// Gets or sets the index of the property in the order by clause.
    /// </summary>
    public int Index { get; set; }
}
