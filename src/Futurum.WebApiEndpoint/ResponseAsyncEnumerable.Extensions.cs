using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Extension methods for ResponseAsyncEnumerable
/// </summary>
public static class ResponseAsyncEnumerableExtensions
{
    /// <summary>
    /// Transform an <see cref="IAsyncEnumerable{T}"/> to a <see cref="ResponseAsyncEnumerable{TData}"/>
    /// </summary>
    public static Result<ResponseAsyncEnumerable<TData>> ToResponseAsyncEnumerable<TData>(this Result<IAsyncEnumerable<TData>> result) =>
        result.Map(x => new ResponseAsyncEnumerable<TData>(x));
}