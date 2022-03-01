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
/// Response dto for async-enumerable
/// </summary>
public record ResponseAsyncEnumerableDto<TData>(IAsyncEnumerable<TData> AsyncEnumerable);