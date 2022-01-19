using Futurum.Core.Functional;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Serilog;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Extension methods for EndpointRouteBuilder
/// </summary>
public static class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// Configures <see cref="WebApiEndpoint"/>
    /// </summary>
    public static IEndpointRouteBuilder UseWebApiEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var metadataCache = endpointRouteBuilder.ServiceProvider.GetService<IWebApiEndpointMetadataCache>();

        var metadataDefinitions = GetAllMetadataDefinitions(metadataCache);

        var webApiEndpointConfiguration = endpointRouteBuilder.ServiceProvider.GetService<WebApiEndpointConfiguration>();

        var endpointRouteOpenApiBuilder = endpointRouteBuilder.ServiceProvider.GetService<IEndpointRouteOpenApiBuilder>();
        var endpointRouteSecurityBuilder = endpointRouteBuilder.ServiceProvider.GetService<IEndpointRouteSecurityBuilder>();

        foreach (var metadataDefinition in metadataDefinitions)
        {
            var route = webApiEndpointConfiguration.RouteFactory(webApiEndpointConfiguration, metadataDefinition.MetadataRouteDefinition);

            var routeHandlerBuilder = endpointRouteBuilder.MapMethods(route, new[] { metadataDefinition.MetadataRouteDefinition.HttpMethod.ToString().ToUpper() }, WebApiEndpointExecutor.ExecuteAsync);

            routeHandlerBuilder.WithMetadata(metadataDefinition);
            routeHandlerBuilder.WithMetadata(webApiEndpointConfiguration);

            endpointRouteSecurityBuilder.Configure(routeHandlerBuilder, webApiEndpointConfiguration, metadataDefinition);

            endpointRouteOpenApiBuilder.Configure(routeHandlerBuilder, metadataDefinition);

            ConfigureExtendedOptions(metadataDefinition, routeHandlerBuilder);

            LogConfiguration(route, metadataDefinition.MetadataRouteDefinition);
        }

        return endpointRouteBuilder;
    }

    private static void LogConfiguration(string route, MetadataRouteDefinition metadataDefinition)
    {
        var eventData = new { Route = route, MetadataDefinition = metadataDefinition };
        Log.Logger.Debug("WebApiEndpoint configuring - {@eventData}", eventData);
    }

    private static IEnumerable<MetadataDefinition> GetAllMetadataDefinitions(IWebApiEndpointMetadataCache metadataCache) =>
        metadataCache.GetAll()
                     .Select(x => x.Value);

    private static void ConfigureExtendedOptions(MetadataDefinition metadataDefinition, RouteHandlerBuilder routeHandlerBuilder)
    {
        metadataDefinition.MetadataRouteDefinition.ExtendedOptions.DoSwitch(builder => builder(routeHandlerBuilder), Function.DoNothing);
    }
}