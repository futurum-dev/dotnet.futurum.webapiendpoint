using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.OpenApi;

public record WebApiEndpointOpenApiVersion(string Title, ApiVersion ApiVersion, bool Deprecated = false);