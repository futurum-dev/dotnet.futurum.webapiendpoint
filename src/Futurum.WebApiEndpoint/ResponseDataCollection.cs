namespace Futurum.WebApiEndpoint;

/// <summary>
/// Response domain for data-collection
/// </summary>
public record ResponseDataCollection<TData>(IEnumerable<TData> Data);

/// <summary>
/// Response dto for data-collection
/// </summary>
public record ResponseDataCollectionDto<TData>(ICollection<TData> Data)
{
    /// <summary>
    /// Data count
    /// </summary>
    public long Count => Data.Count;
}