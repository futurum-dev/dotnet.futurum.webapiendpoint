using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Internal.AspNetCore;

internal static class HttpRequestExtensions
{
    public static Task<Result<string>> ReadPlainTextBodyAsync(this HttpRequest httpRequest)
    {
        async Task<string> Execute()
        {
            using var streamReader = new StreamReader(httpRequest.Body);
            return await streamReader.ReadToEndAsync();
        }

        return Result.TryAsync(Execute, () => "Failed to read request as plain text");
    }
}