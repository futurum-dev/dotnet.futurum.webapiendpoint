namespace Futurum.WebApiEndpoint;

/// <summary>
/// Response domain for data-collection with <typeparamref name="TApiEndpoint"/>
/// </summary>
public record ResponseDataCollection<TApiEndpoint, TData>(IEnumerable<TData> Data);

/// <summary>
/// Response domain for data-collection
/// </summary>
public record ResponseDataCollection<TData>(IEnumerable<TData> Data)
{
    internal ResponseDataCollection<TApiEndpoint, TData> ToApiEndpoint<TApiEndpoint>() => new(Data);
}

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