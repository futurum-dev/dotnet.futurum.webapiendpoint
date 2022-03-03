using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Extension methods for ResponseEmpty
/// </summary>
public static class ResponseEmptyExtensions
{
    /// <summary>
    /// Return a <see cref="ResponseEmpty"/>
    /// </summary>
    public static Task<Result<ResponseEmpty>> ToResponseEmptyAsync(this Result result) =>
        result.Map(ResponseEmpty.Default).ToResultAsync();
    
    /// <summary>
    /// Return a <see cref="ResponseEmpty"/>
    /// </summary>
    public static Task<Result<ResponseEmpty>> ToResponseEmptyAsync<T>(this Result<T> result) =>
        result.Map(_ => ResponseEmpty.Default).ToResultAsync();
    
    /// <summary>
    /// Return a <see cref="ResponseEmpty"/>
    /// </summary>
    public static Task<Result<ResponseEmpty>> ToResponseEmptyAsync(this Task<Result> result) =>
        result.MapAsync(ResponseEmpty.Default);
    
    /// <summary>
    /// Return a <see cref="ResponseEmpty"/>
    /// </summary>
    public static Task<Result<ResponseEmpty>> ToResponseEmptyAsync<T>(this Task<Result<T>> result) =>
        result.MapAsync(_ => ResponseEmpty.Default);
}