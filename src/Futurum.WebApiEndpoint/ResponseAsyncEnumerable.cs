using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Response domain for async-enumerable with <typeparamref name="TApiEndpoint"/>
/// </summary>
public record ResponseAsyncEnumerable<TApiEndpoint, TData>(IAsyncEnumerable<TData> Data);

/// <summary>
/// Response domain for async-enumerable
/// </summary>
public record ResponseAsyncEnumerable<TData>(IAsyncEnumerable<TData> Data)
{
    internal ResponseAsyncEnumerable<TApiEndpoint, TData> ToApiEndpoint<TApiEndpoint>() => new(Data);
}

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

/// <summary>
/// Response dto for async-enumerable
/// </summary>
public record ResponseAsyncEnumerableDto<T>(IAsyncEnumerable<T> AsyncEnumerable);

internal class ResponseAsyncEnumerableMapper<TApiEndpoint, TData, TDataDto, TDataMapper> : IWebApiEndpointResponseMapper<ResponseAsyncEnumerable<TApiEndpoint, TData>, ResponseAsyncEnumerableDto<TDataDto>>
    where TDataMapper : IWebApiEndpointDataMapper<TData, TDataDto>
{
    private readonly TDataMapper _dataMapper;

    public ResponseAsyncEnumerableMapper(TDataMapper dataMapper)
    {
        _dataMapper = dataMapper;
    }

    public ResponseAsyncEnumerableDto<TDataDto> Map(ResponseAsyncEnumerable<TApiEndpoint, TData> domain) => 
        new(Map(domain.Data));

    private async IAsyncEnumerable<TDataDto> Map(IAsyncEnumerable<TData> domain)
    {
        await foreach (var item in domain)
        {
            yield return _dataMapper.Map(item);
        }
    }
}