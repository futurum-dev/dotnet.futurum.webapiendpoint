using Futurum.ApiEndpoint.DebugLogger;
using Futurum.Core.Option;

namespace Futurum.WebApiEndpoint.Metadata;

/// <summary>
/// Builder for Query <see cref="MetadataRouteDefinition"/>
/// </summary>
public class QueryMetadataRouteDefinitionInitialBuilder : IMetadataRouteDefinitionBuilder
{
    private readonly Type _apiEndpointType;

    private IMetadataRouteDefinitionBuilder _metadataRouteDefinitionBuilder;

    public QueryMetadataRouteDefinitionInitialBuilder(Type apiEndpointType)
    {
        _apiEndpointType = apiEndpointType;
    }
    
    /// <summary>
    /// Configure <paramref name="route"/>
    /// <para />
    /// Defaults values to the following:
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         <see cref="MetadataRouteDefinition.SuccessStatusCode"/> = 200
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         <see cref="MetadataRouteDefinition.FailedStatusCode"/> = 503
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public MetadataRouteDefinitionBuilder Route(string route)
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, route, null, new(),
                                                                  null, 200, 503, Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataRouteDefinitionBuilder = new MetadataRouteDefinitionBuilder(_apiEndpointType, metadataRouteDefinition);
        _metadataRouteDefinitionBuilder = metadataRouteDefinitionBuilder;
        return metadataRouteDefinitionBuilder;
    }
    
    /// <summary>
    /// Configure <paramref name="route"/> with parameters
    /// <para />
    /// Defaults values to the following:
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         <see cref="MetadataRouteDefinition.SuccessStatusCode"/> = 200
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         <see cref="MetadataRouteDefinition.FailedStatusCode"/> = 404
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public MetadataRouteDefinitionBuilder RouteWithParameters(string route)
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, route, null, new(),
                                                                  null, 200, 404, Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataRouteDefinitionBuilder = new MetadataRouteDefinitionBuilder(_apiEndpointType, metadataRouteDefinition);
        _metadataRouteDefinitionBuilder = metadataRouteDefinitionBuilder;
        return metadataRouteDefinitionBuilder;
    }

    /// <summary>
    /// Configure <paramref name="route"/> with <paramref name="parameterDefinitions"/>
    /// <para>You only need to specify the parameters that DO NOT use <see cref="MapFromAttribute"/></para>
    /// <para />
    /// Defaults values to the following:
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         <see cref="MetadataRouteDefinition.SuccessStatusCode"/> = 200
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         <see cref="MetadataRouteDefinition.FailedStatusCode"/> = 404
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public MetadataRouteDefinitionBuilder Route(string route, params (string name, MetadataRouteParameterDefinitionType parameterDefinitionType, Type type)[] parameterDefinitions)
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, route, null,
                                                                  parameterDefinitions.Select(x => new MetadataRouteParameterDefinition(x.name, x.parameterDefinitionType, x.type)).ToList(),
                                                                  null, 200, 404, Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);
        
        var metadataRouteDefinitionBuilder = new MetadataRouteDefinitionBuilder(_apiEndpointType, metadataRouteDefinition);
        _metadataRouteDefinitionBuilder = metadataRouteDefinitionBuilder;
        return metadataRouteDefinitionBuilder;
    }

    IEnumerable<MetadataRouteDefinition> IMetadataRouteDefinitionBuilder.Build() =>
        _metadataRouteDefinitionBuilder.Build();

    ApiEndpointDebugNode IMetadataRouteDefinitionBuilder.Debug() =>
        _metadataRouteDefinitionBuilder.Debug();
}