using System.Net.Mime;
using System.Text.Json;

using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Internal.AspNetCore;

internal static class HttpResponseExtensions
{
    public static Task<Result> TrySendResponseStreamAsync(this HttpResponse httpResponse, Stream stream, int responseStatusCode, string? fileName = null, long? fileLengthBytes = null,
                                                       string contentType = MediaTypeNames.Application.Octet, CancellationToken cancellation = default)
    {
        async Task Execute()
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream), "The supplied stream cannot be null!");
            }

            if (stream.Position > 0 && !stream.CanSeek)
            {
                throw new ArgumentException("The supplied stream is not seekable and the position can't be set back to 0.");
            }

            httpResponse.StatusCode = responseStatusCode;
            httpResponse.ContentType = contentType;
            httpResponse.ContentLength = fileLengthBytes;

            if (fileName is not null)
            {
                httpResponse.Headers.Add("Content-Disposition", $"attachment; filename={fileName}");
            }

            await stream.CopyToAsync(httpResponse.Body, 64 * 1024, cancellation);

            stream.Close();
        }

        return Result.TryAsync(Execute, () => "Failed to send response stream");
    }

    public static Task<Result> TrySendResponseFileAsync(this HttpResponse httpResponse, FileInfo fileInfo, int responseStatusCode, string contentType = MediaTypeNames.Application.Octet,
                                                     CancellationToken cancellation = default)
    {
        async Task Execute()
        {
            await using var fileStream = fileInfo.OpenRead();
            await httpResponse.TrySendResponseStreamAsync(fileStream, responseStatusCode, fileInfo.Name, fileInfo.Length, contentType, cancellation);
        }

        return Result.TryAsync(Execute, () => "Failed to send response file");
    }

    public static Task<Result> TrySendResponseBytesAsync(this HttpResponse httpResponse, byte[] bytes, int responseStatusCode, string? fileName = null, long? fileLengthBytes = null,
                                                      string contentType = MediaTypeNames.Application.Octet, CancellationToken cancellation = default)
    {
        async Task Execute()
        {
            await using var memoryStream = new MemoryStream(bytes);
            await httpResponse.TrySendResponseStreamAsync(memoryStream, responseStatusCode, fileName, fileLengthBytes, contentType, cancellation);
        }

        return Result.TryAsync(Execute, () => "Failed to send response bytes");
    }

    public static Task<Result> TryWriteAsJsonAsync<TResponse>(this HttpResponse httpResponse, TResponse response, JsonSerializerOptions jsonSerializerOptions, int responseStatusCode,
                                                           CancellationToken cancellation)
    {
        Task Execute()
        {
            httpResponse.StatusCode = responseStatusCode;
            return httpResponse.WriteAsJsonAsync(response, jsonSerializerOptions, cancellation);
        }

        return Result.TryAsync(Execute, () => "Failed to write as Json");
    }

    public static Task<Result> TryWriteAsyncEnumerableAsJsonAsync(this HttpResponse httpResponse, object source, Type type, JsonSerializerOptions jsonSerializerOptions, int responseStatusCode,
                                                               CancellationToken cancellation)
    {
        Task Execute()
        {
            httpResponse.StatusCode = responseStatusCode;
            return httpResponse.WriteAsJsonAsync(source, type, jsonSerializerOptions, cancellation);
        }

        return Result.TryAsync(Execute, () => "Failed to write AsyncEnumerable as Json");
    }
}