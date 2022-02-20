using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Futurum.WebApiEndpoint.Internal;

internal partial interface IWebApiEndpointHttpContextDispatcher
{
}

internal partial class WebApiEndpointHttpContextDispatcher : IWebApiEndpointHttpContextDispatcher
{
    private readonly IOptions<JsonOptions> _serializationOptions;

    public WebApiEndpointHttpContextDispatcher(IOptions<JsonOptions> serializationOptions)
    {
        _serializationOptions = serializationOptions;
    }
}