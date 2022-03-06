using System.Diagnostics.CodeAnalysis;

using Futurum.ApiEndpoint;
using Futurum.Microsoft.Extensions.DependencyInjection;

namespace Futurum.WebApiEndpoint;

[ExcludeFromCodeCoverage]
public class WebApiEndpointSerilogLoggerModule : IModule
{
    public void Load(IServiceCollection services)
    {
        services.AddSingleton<IWebApiEndpointLogger, WebApiEndpointLogger>();
        services.AddSingleton<IApiEndpointLogger, WebApiEndpointLogger>();
    }
}