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

internal record RequestPlainTextDto;