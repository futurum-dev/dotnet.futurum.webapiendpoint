using System.Net.Mime;
using System.Text.Json;

using Futurum.Core.Option;
using Futurum.Core.Result;

using Microsoft.AspNetCore.Http.Extensions;

namespace Futurum.WebApiEndpoint.Internal.AspNetCore;

internal static class HttpResponseExtensions
{
    public static Task<Result> TrySendResponseStreamAsync(this HttpResponse httpResponse, Stream stream, Option<Range> optionRange, int responseStatusCode, string? fileName = null, long? fileLengthBytes = null,
                                                          string contentType = MediaTypeNames.Application.Octet, CancellationToken cancellation = default)
    {
        async Task<Result> WithRange(Range range)
        {
            var from = range.From.GetValueOrDefault(0);
            var to = range.To.GetValueOrDefault(stream.Length);

            stream.Seek(from, SeekOrigin.Begin);

            var length = to - from + 1;

            httpResponse.ContentLength = length;

            await StreamCopyOperation.CopyToAsync(stream, httpResponse.Body, length, 64 * 1024, cancellation);

            stream.Close();

            return Result.Ok();
        }

        async Task<Result> WithoutRange()
        {
            stream.Seek(0, SeekOrigin.Begin);

            httpResponse.ContentLength = stream.Length;

            await stream.CopyToAsync(httpResponse.Body, 64 * 1024, cancellation);

            stream.Close();

            return Result.Ok();
        }

        Task<Result> Execute()
        {
            if (stream is null)
            {
                return Result.FailAsync("The supplied stream cannot be null");
            }

            if (stream.Position > 0 && !stream.CanSeek)
            {
                return Result.FailAsync("The supplied stream is not seekable and the position can't be set back to 0.");
            }

            httpResponse.StatusCode = responseStatusCode;
            httpResponse.ContentType = contentType;
            httpResponse.ContentLength = fileLengthBytes;

            if (fileName is not null)
            {
                httpResponse.Headers.Add("Content-Disposition", $"attachment; filename={fileName}");
            }

            return optionRange.Switch(WithRange, WithoutRange);
        }

        return Result.TryAsync(Execute, () => "Failed to send response stream");
    }

    public static Task<Result> TrySendResponseFileAsync(this HttpResponse httpResponse, FileInfo fileInfo, Option<Range> range, int responseStatusCode, string contentType = MediaTypeNames.Application.Octet,
                                                        CancellationToken cancellation = default)
    {
        async Task Execute()
        {
            await using var fileStream = fileInfo.OpenRead();
            await httpResponse.TrySendResponseStreamAsync(fileStream, range, responseStatusCode, fileInfo.Name, fileInfo.Length, contentType, cancellation);
        }

        return Result.TryAsync(Execute, () => "Failed to send response file");
    }

    public static Task<Result> TrySendResponseBytesAsync(this HttpResponse httpResponse, byte[] bytes, Option<Range> range, int responseStatusCode, string? fileName = null, long? fileLengthBytes = null,
                                                         string contentType = MediaTypeNames.Application.Octet, CancellationToken cancellation = default)
    {
        async Task Execute()
        {
            await using var memoryStream = new MemoryStream(bytes);
            await httpResponse.TrySendResponseStreamAsync(memoryStream, range, responseStatusCode, fileName, fileLengthBytes, contentType, cancellation);
        }

        return Result.TryAsync(Execute, () => "Failed to send response bytes");
    }

    public static Task<Result> TryWriteAsJsonAsync<TResponse>(this HttpResponse httpResponse, TResponse response, JsonSerializerOptions jsonSerializerOptions, int responseStatusCode,
                                                              CancellationToken cancellation)
    {
        Task Execute()
        {
            httpResponse.StatusCode = responseStatusCode;
            httpResponse.ContentType = MediaTypeNames.Application.Json;
            
            return JsonSerializer.SerializeAsync(httpResponse.Body, response, jsonSerializerOptions, cancellation);
        }

        return Result.TryAsync(Execute, () => "Failed to write as Json");
    }

    public static Task<Result> TryWriteAsProblemJsonAsync<TResponse>(this HttpResponse httpResponse, TResponse response, JsonSerializerOptions jsonSerializerOptions, int responseStatusCode,
                                                              CancellationToken cancellation)
    {
        Task Execute()
        {
            httpResponse.StatusCode = responseStatusCode;
            httpResponse.ContentType = WebApiEndpointContentType.ProblemJson;
            
            return JsonSerializer.SerializeAsync(httpResponse.Body, response, jsonSerializerOptions, cancellation);
        }

        return Result.TryAsync(Execute, () => "Failed to write as Problem Json");
    }

    public static Task<Result> TryWriteAsyncEnumerableAsJsonAsync<T>(this HttpResponse httpResponse, T source, JsonSerializerOptions jsonSerializerOptions, int responseStatusCode,
                                                                  CancellationToken cancellation)
    {
        Task Execute()
        {
            httpResponse.StatusCode = responseStatusCode;
            httpResponse.ContentType = MediaTypeNames.Application.Json;
            
            return JsonSerializer.SerializeAsync(httpResponse.Body, source, jsonSerializerOptions, cancellation);
        }

        return Result.TryAsync(Execute, () => "Failed to write AsyncEnumerable as Json");
    }
}