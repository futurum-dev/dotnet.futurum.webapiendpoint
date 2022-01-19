using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Request domain for plain text with <typeparamref name="TApiEndpoint"/>
/// </summary>
public record RequestPlainText<TApiEndpoint>(string Body)
{
    internal RequestPlainText ToNonApiEndpoint() => new(Body);
}

/// <summary>
/// Request domain for plain text
/// </summary>
public record RequestPlainText(string Body);

/// <summary>
/// Request dto for plain text
/// </summary>
public record RequestPlainTextDto(string Body);

internal class RequestPlainTextMapper<TApiEndpoint> : IWebApiEndpointRequestMapper<RequestPlainTextDto, RequestPlainText<TApiEndpoint>>
{
    public Result<RequestPlainText<TApiEndpoint>> Map(HttpContext httpContext, RequestPlainTextDto dto) =>
        new RequestPlainText<TApiEndpoint>(dto.Body).ToResultOk();
}