using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

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