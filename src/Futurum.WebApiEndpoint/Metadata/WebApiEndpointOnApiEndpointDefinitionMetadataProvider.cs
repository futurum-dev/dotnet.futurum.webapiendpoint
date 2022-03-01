using System.Reflection;

using Futurum.ApiEndpoint;
using Futurum.ApiEndpoint.DebugLogger;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;

namespace Futurum.WebApiEndpoint.Metadata;

internal static class WebApiEndpointOnApiEndpointDefinitionMetadataProvider
{
    public static IEnumerable<MetadataDefinition> GetMetadata(IEnumerable<Assembly> assemblies)
    {
        var apiEndpointDefinitions = assemblies.SelectMany(s => s.GetTypes())
                                               .Where(p => typeof(IApiEndpointDefinition).IsAssignableFrom(p))
                                               .OrderBy(x => x.FullName)
                                               .Select(Activator.CreateInstance)
                                               .Cast<IApiEndpointDefinition>();

        return GetMetadata(apiEndpointDefinitions);
    }
    
    public static IEnumerable<MetadataDefinition> GetMetadata(IEnumerable<IApiEndpointDefinition> apiEndpointDefinitions)
    {
        ApiEndpointDefinitionBuilder apiEndpointDefinitionBuilder = new();

        foreach (var apiEndpointDefinition in apiEndpointDefinitions)
        {
            apiEndpointDefinition.Configure(apiEndpointDefinitionBuilder);
        }

        return apiEndpointDefinitionBuilder.Get()
                                           .Filter(x => x.MetadataDefinition is MetadataRouteDefinition)
                                           .FlatMap(GetMetadataDefinitions)
                                           .MapSwitch(Enumerable.ToList, () => new List<MetadataDefinition>())
                                           .EnhanceWithError(() => $"Failed to set cache in {nameof(WebApiEndpointOnApiEndpointDefinitionMetadataProvider)}")
                                           .Unwrap();
    }

    public static IEnumerable<ApiEndpointDebugNode> GetDebug(IEnumerable<Assembly> assemblies)
    {
        var apiEndpointDefinitions = assemblies.SelectMany(s => s.GetTypes())
                                               .Where(p => typeof(IApiEndpointDefinition).IsAssignableFrom(p))
                                               .OrderBy(x => x.FullName)
                                               .Select(Activator.CreateInstance)
                                               .Cast<IApiEndpointDefinition>();

        ApiEndpointDefinitionBuilder apiEndpointDefinitionBuilder = new();

        foreach (var apiEndpointDefinition in apiEndpointDefinitions)
        {
            apiEndpointDefinition.Configure(apiEndpointDefinitionBuilder);
        }

        return apiEndpointDefinitionBuilder.Debug();
    }

    private static IEnumerable<MetadataDefinition> GetMetadataDefinitions(ApiEndpointDefinition apiEndpointDefinition)
    {
        var metadataRouteDefinition = apiEndpointDefinition.MetadataDefinition as MetadataRouteDefinition;
        var apiEndpointType = apiEndpointDefinition.ApiEndpointType;

        if (apiEndpointType.IsClosedTypeOf(typeof(ICommandWebApiEndpoint<,,,,,>)))
        {
            var apiEndpointInterfaceType = apiEndpointType.GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommandWebApiEndpoint<,,,,,>));

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.Get(apiEndpointInterfaceType, apiEndpointType);

            var metadataMapFromDefinition = GetMetadataMapFromDefinition(metadataTypeDefinition.UnderlyingRequestDtoType);
            var metadataMapFromMultipartDefinition = GetMetadataMapFromMultipartDefinition(metadataTypeDefinition.UnderlyingRequestDtoType);

            yield return new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);
        }
        
        if (apiEndpointType.IsClosedTypeOf(typeof(IQueryWebApiEndpoint<,,,,,>)))
        {
            var apiEndpointInterfaceType = apiEndpointType.GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IQueryWebApiEndpoint<,,,,,>));

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.Get(apiEndpointInterfaceType, apiEndpointType);

            var metadataMapFromDefinition = GetMetadataMapFromDefinition(metadataTypeDefinition.UnderlyingRequestDtoType);
            var metadataMapFromMultipartDefinition = GetMetadataMapFromMultipartDefinition(metadataTypeDefinition.UnderlyingRequestDtoType);

            yield return new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);
        }
    }

    private static MetadataMapFromDefinition? GetMetadataMapFromDefinition(Type requestDtoType)
    {
        var parameters = WebApiEndpointMetadataTypeService.GetMapFromProperties(requestDtoType)
                         .Select(x => new MetadataMapFromParameterDefinition(x.propertyInfo.Name, x.propertyInfo, x.mapFromAttribute))
                         .ToList();

        return parameters.Any() ? new MetadataMapFromDefinition(parameters) : null;
    }

    private static MetadataMapFromMultipartDefinition? GetMetadataMapFromMultipartDefinition(Type requestDtoType)
    {
        var parameters = WebApiEndpointMetadataTypeService.GetMapFromMultipartProperties(requestDtoType)
                         .Select(x => new MetadataMapFromMultipartParameterDefinition(x.propertyInfo.Name, x.propertyInfo, x.mapFromMultipartAttribute))
                         .ToList();

        return parameters.Any() ? new MetadataMapFromMultipartDefinition(parameters) : null;
    }
}