using Futurum.ApiEndpoint.DebugLogger;
using Futurum.Core.Option;

namespace Futurum.WebApiEndpoint.Metadata;

/// <summary>
/// Builder for Command <see cref="MetadataRouteDefinition"/>
/// </summary>
public class CommandMetadataRouteDefinitionInitialBuilder : IMetadataRouteDefinitionBuilder
{
    private readonly Type _apiEndpointType;

    private IMetadataRouteDefinitionBuilder _metadataRouteDefinitionBuilder;

    public CommandMetadataRouteDefinitionInitialBuilder(Type apiEndpointType)
    {
        _apiEndpointType = apiEndpointType;
    }
    
    /// <summary>
    /// Configure Post <paramref name="route"/>
    /// </summary>
    public MetadataRouteDefinitionBuilder Post(string route)
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Post, route, null, new(),
                                                                  null, 201, 400, Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataRouteDefinitionBuilder = new MetadataRouteDefinitionBuilder(_apiEndpointType, metadataRouteDefinition);
        _metadataRouteDefinitionBuilder = metadataRouteDefinitionBuilder;
        return metadataRouteDefinitionBuilder;
    }

    /// <summary>
    /// Configure Post <paramref name="route"/> with <paramref name="parameterDefinitions"/>
    /// <para>You only need to specify the parameters that DO NOT use <see cref="MapFromAttribute"/></para>
    /// </summary>
    public MetadataRouteDefinitionBuilder Post(string route, params (string name, MetadataRouteParameterDefinitionType parameterDefinitionType, Type type)[] parameterDefinitions)
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Post, route, null,
                                                                  parameterDefinitions.Select(x => new MetadataRouteParameterDefinition(x.name, x.parameterDefinitionType, x.type)).ToList(),
                                                                  null, 201, 400, Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);
        
        var metadataRouteDefinitionBuilder = new MetadataRouteDefinitionBuilder(_apiEndpointType, metadataRouteDefinition);
        _metadataRouteDefinitionBuilder = metadataRouteDefinitionBuilder;
        return metadataRouteDefinitionBuilder;
    }
    
    /// <summary>
    /// Configure Put <paramref name="route"/>
    /// </summary>
    public MetadataRouteDefinitionBuilder Put(string route)
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Put, route, null, new(),
                                                                  null, 201, 404, Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);
        
        var metadataRouteDefinitionBuilder = new MetadataRouteDefinitionBuilder(_apiEndpointType, metadataRouteDefinition);
        _metadataRouteDefinitionBuilder = metadataRouteDefinitionBuilder;
        return metadataRouteDefinitionBuilder;
    }

    /// <summary>
    /// Configure Put <paramref name="route"/> with <paramref name="parameterDefinitions"/>
    /// <para>You only need to specify the parameters that DO NOT use <see cref="MapFromAttribute"/></para>
    /// </summary>
    public MetadataRouteDefinitionBuilder Put(string route, params (string name, MetadataRouteParameterDefinitionType parameterDefinitionType, Type type)[] parameterDefinitions)
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Put, route, null,
                                                                  parameterDefinitions.Select(x => new MetadataRouteParameterDefinition(x.name, x.parameterDefinitionType, x.type)).ToList(),
                                                                  null, 201, 404, Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);
        
        var metadataRouteDefinitionBuilder = new MetadataRouteDefinitionBuilder(_apiEndpointType, metadataRouteDefinition);
        _metadataRouteDefinitionBuilder = metadataRouteDefinitionBuilder;
        return metadataRouteDefinitionBuilder;
    }
    
    /// <summary>
    /// Configure Patch <paramref name="route"/>
    /// </summary>
    public MetadataRouteDefinitionBuilder Patch(string route)
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Patch, route, null, new(),
                                                                  null, 200, 404, Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);
        
        var metadataRouteDefinitionBuilder = new MetadataRouteDefinitionBuilder(_apiEndpointType, metadataRouteDefinition);
        _metadataRouteDefinitionBuilder = metadataRouteDefinitionBuilder;
        return metadataRouteDefinitionBuilder;
    }

    /// <summary>
    /// Configure Patch <paramref name="route"/> with <paramref name="parameterDefinitions"/>
    /// <para>You only need to specify the parameters that DO NOT use <see cref="MapFromAttribute"/></para>
    /// </summary>
    public MetadataRouteDefinitionBuilder Patch(string route, params (string name, MetadataRouteParameterDefinitionType parameterDefinitionType, Type type)[] parameterDefinitions)
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Patch, route, null,
                                                                  parameterDefinitions.Select(x => new MetadataRouteParameterDefinition(x.name, x.parameterDefinitionType, x.type)).ToList(),
                                                                  null, 200, 404, Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);
        
        var metadataRouteDefinitionBuilder = new MetadataRouteDefinitionBuilder(_apiEndpointType, metadataRouteDefinition);
        _metadataRouteDefinitionBuilder = metadataRouteDefinitionBuilder;
        return metadataRouteDefinitionBuilder;
    }
    
    /// <summary>
    /// Configure Delete <paramref name="route"/>
    /// </summary>
    public MetadataRouteDefinitionBuilder Delete(string route)
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Delete, route, null, new(),
                                                                  null, 200, 404, Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);
        
        var metadataRouteDefinitionBuilder = new MetadataRouteDefinitionBuilder(_apiEndpointType, metadataRouteDefinition);
        _metadataRouteDefinitionBuilder = metadataRouteDefinitionBuilder;
        return metadataRouteDefinitionBuilder;
    }

    /// <summary>
    /// Configure Delete <paramref name="route"/> with <paramref name="parameterDefinitions"/>
    /// <para>You only need to specify the parameters that DO NOT use <see cref="MapFromAttribute"/></para>
    /// </summary>
    public MetadataRouteDefinitionBuilder Delete(string route, params (string name, MetadataRouteParameterDefinitionType parameterDefinitionType, Type type)[] parameterDefinitions)
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Delete, route, null,
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