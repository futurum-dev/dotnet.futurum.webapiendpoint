using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public static class ResponseEmptyExtensions
{
    public static Task<Result<ResponseEmpty>> ToResponseEmptyAsync(this Result result) =>
        result.Map(ResponseEmpty.Default).ToResultAsync();
    
    public static Task<Result<ResponseEmpty>> ToResponseEmptyAsync<T>(this Result<T> result) =>
        result.Map(_ => ResponseEmpty.Default).ToResultAsync();
    
    public static Task<Result<ResponseEmpty>> ToResponseEmptyAsync(this Task<Result> result) =>
        result.MapAsync(ResponseEmpty.Default);
    
    public static Task<Result<ResponseEmpty>> ToResponseEmptyAsync<T>(this Task<Result<T>> result) =>
        result.MapAsync(_ => ResponseEmpty.Default);
}