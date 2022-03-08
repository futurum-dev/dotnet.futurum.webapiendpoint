namespace Futurum.WebApiEndpoint;

/// <summary>
/// Response domain for redirect
/// </summary>
public record ResponseRedirect(string Location, bool Permanent);

/// <summary>
/// Response dto for redirect
/// </summary>
public record ResponseRedirectDto;