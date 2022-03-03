namespace Futurum.WebApiEndpoint;

/// <summary>
/// Response domain for async-enumerable
/// </summary>
public record ResponseAsyncEnumerable<TData>(IAsyncEnumerable<TData> Data);

/// <summary>
/// Response dto for async-enumerable
/// </summary>
public record ResponseAsyncEnumerableDto<TData> : IResponseWrapperDto<TData>;