namespace Futurum.WebApiEndpoint;

/// <summary>
/// Response domain for empty-json with <typeparamref name="TApiEndpoint"/>
/// </summary>
public record ResponseEmptyJson<TApiEndpoint>;

/// <summary>
/// Response domain for empty-json
/// </summary>
public record ResponseEmptyJson
{
    internal ResponseEmptyJson<TApiEndpoint> ToApiEndpoint<TApiEndpoint>() => new();
}

/// <summary>
/// Response dto for empty-json
/// </summary>
public record ResponseEmptyJsonDto;