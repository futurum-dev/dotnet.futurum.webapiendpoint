using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Internal.AspNetCore;

internal static class HttpRequestExtensions
{
    public static Task<Result<string>> TryReadPlainTextBodyAsync(this HttpRequest httpRequest)
    {
        async Task<string> Execute()
        {
            using var streamReader = new StreamReader(httpRequest.Body);
            return await streamReader.ReadToEndAsync();
        }

        return Result.TryAsync(Execute, "Failed to read request as plain text");
    }
    
    public static Task<Result<IEnumerable<IFormFile>>> TryReadUploadFilesAsync(this HttpRequest httpRequest, CancellationToken cancellationToken)
    {
        async Task<IEnumerable<IFormFile>> Execute()
        {
            var form = await httpRequest.ReadFormAsync(cancellationToken);
            return form.Files.ToList();
        }

        if (!httpRequest.HasFormContentType)
            return Result.FailAsync<IEnumerable<IFormFile>>("Failed to read upload files");

        return Result.TryAsync(Execute, "Failed to read upload files");
    }
}