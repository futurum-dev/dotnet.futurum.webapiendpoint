using Futurum.Core.Linq;
using Futurum.Core.Result;

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
/// Extension methods for ResponseDataCollection
/// </summary>
public static class ResponseDataCollectionExtensions
{
    /// <summary>
    /// Transform an <see cref="IEnumerable{T}"/> to a <see cref="ResponseDataCollection{TData}"/>
    /// </summary>
    public static Task<Result<ResponseDataCollection<TData>>> ToResponseDataCollectionAsync<TData>(this IEnumerable<TData> source) =>
        new ResponseDataCollection<TData>(source).ToResultOkAsync();

    /// <summary>
    /// Transform an <see cref="IEnumerable{T}"/> to a <see cref="ResponseDataCollection{TData}"/>
    /// </summary>
    public static Task<Result<ResponseDataCollection<TData>>> ToResponseDataCollectionAsync<TData>(this Result<IEnumerable<TData>> result) =>
        result.Map(x => new ResponseDataCollection<TData>(x))
              .ToResultAsync();

    /// <summary>
    /// Transform an <see cref="IEnumerable{T}"/> to a <see cref="ResponseDataCollection{TData}"/>
    /// </summary>
    public static Task<Result<ResponseDataCollection<TData>>> ToResponseDataCollectionAsync<TData>(this Task<Result<IEnumerable<TData>>> result) =>
        result.MapAsync(x => new ResponseDataCollection<TData>(x));
}

/// <summary>
/// Response dto for data-collection
/// </summary>
public record ResponseDataCollectionDto<T>(ICollection<T> Data)
{
    /// <summary>
    /// Data count
    /// </summary>
    public long Count => Data.Count;
}

internal class ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper> : IWebApiEndpointResponseMapper<ResponseDataCollection<TApiEndpoint, TData>, ResponseDataCollectionDto<TDataDto>>
    where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
{
    private readonly TResponseDataMapper _dataMapper;

    public ResponseDataCollectionMapper(TResponseDataMapper dataMapper)
    {
        _dataMapper = dataMapper;
    }

    public ResponseDataCollectionDto<TDataDto> Map(ResponseDataCollection<TApiEndpoint, TData> domain) => 
        new(domain.Data.Select(_dataMapper.Map).AsList());
}