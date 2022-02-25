using System.Reflection;

using Futurum.ApiEndpoint;
using Futurum.ApiEndpoint.DebugLogger;
using Futurum.Core.Linq;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;

namespace Futurum.WebApiEndpoint.Metadata;

internal static class WebApiEndpointOnApiEndpointDefinitionMetadataProvider
{
    public static IEnumerable<MetadataDefinition> GetMetadata(IEnumerable<Assembly> assemblies)
    {
        var apiEndpointDefinitions = assemblies.SelectMany(s => s.GetTypes())
                                               .Where(p => typeof(IApiEndpointDefinition).IsAssignableFrom(p))
                                               .OrderBy(x => x.GetType().FullName)
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

        if (apiEndpointType.IsClosedTypeOf(typeof(IQueryWebApiEndpoint<,,>)))
        {
            var apiEndpointInterfaceType = apiEndpointType.GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IQueryWebApiEndpoint<,,>));

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForQueryWithoutRequest(apiEndpointInterfaceType, apiEndpointType);

            var metadataMapFromDefinition = GetMetadataMapFromDefinition(metadataTypeDefinition.RequestDtoType);
            var metadataMapFromMultipartDefinition = GetMetadataMapFromMultipartDefinition(metadataTypeDefinition.RequestDtoType);

            metadataRouteDefinition = EnrichMetadataRouteDefinitionWithParameterDefinitions(metadataTypeDefinition, metadataRouteDefinition);

            yield return new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);
        }

        if (apiEndpointType.IsClosedTypeOf(typeof(IQueryWebApiEndpoint<,,,,>)))
        {
            var apiEndpointInterfaceType = apiEndpointType.GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IQueryWebApiEndpoint<,,,,>));

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForQueryWithoutRequestDto(apiEndpointInterfaceType, apiEndpointType);

            var metadataMapFromDefinition = GetMetadataMapFromDefinition(metadataTypeDefinition.RequestDtoType);
            var metadataMapFromMultipartDefinition = GetMetadataMapFromMultipartDefinition(metadataTypeDefinition.RequestDtoType);

            metadataRouteDefinition = EnrichMetadataRouteDefinitionWithParameterDefinitions(metadataTypeDefinition, metadataRouteDefinition);

            yield return new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);
        }

        if (apiEndpointType.IsClosedTypeOf(typeof(IQueryWebApiEndpoint<,,,,,>)))
        {
            var apiEndpointInterfaceType = apiEndpointType.GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IQueryWebApiEndpoint<,,,,,>));

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForQueryWithRequestDto(apiEndpointInterfaceType, apiEndpointType);

            var metadataMapFromDefinition = GetMetadataMapFromDefinition(metadataTypeDefinition.RequestDtoType);
            var metadataMapFromMultipartDefinition = GetMetadataMapFromMultipartDefinition(metadataTypeDefinition.RequestDtoType);

            metadataRouteDefinition = EnrichMetadataRouteDefinitionWithParameterDefinitions(metadataTypeDefinition, metadataRouteDefinition);

            yield return new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);
        }

        if (apiEndpointType.IsClosedTypeOf(typeof(ICommandWebApiEndpoint<,,,,,>)))
        {
            var apiEndpointInterfaceType = apiEndpointType.GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommandWebApiEndpoint<,,,,,>));

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForCommandWithRequestWithResponse(apiEndpointInterfaceType, apiEndpointType);

            var metadataMapFromDefinition = GetMetadataMapFromDefinition(metadataTypeDefinition.RequestDtoType);
            var metadataMapFromMultipartDefinition = GetMetadataMapFromMultipartDefinition(metadataTypeDefinition.RequestDtoType);

            metadataRouteDefinition = EnrichMetadataRouteDefinitionWithParameterDefinitions(metadataTypeDefinition, metadataRouteDefinition);

            yield return new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);
        }

        if (apiEndpointType.IsClosedTypeOf(typeof(ICommandWebApiEndpoint<,,,,>)))
        {
            var apiEndpointInterfaceType = apiEndpointType.GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommandWebApiEndpoint<,,,,>));

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForCommandWithoutRequestWithResponse(apiEndpointInterfaceType, apiEndpointType);

            var metadataMapFromDefinition = GetMetadataMapFromDefinition(metadataTypeDefinition.RequestDtoType);
            var metadataMapFromMultipartDefinition = GetMetadataMapFromMultipartDefinition(metadataTypeDefinition.RequestDtoType);

            metadataRouteDefinition = EnrichMetadataRouteDefinitionWithParameterDefinitions(metadataTypeDefinition, metadataRouteDefinition);

            yield return new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);
        }

        if (apiEndpointType.IsClosedTypeOf(typeof(ICommandWebApiEndpoint<,,>)))
        {
            var apiEndpointInterfaceType = apiEndpointType.GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommandWebApiEndpoint<,,>));

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForCommandWithoutResponse(apiEndpointInterfaceType, apiEndpointType);

            var metadataMapFromDefinition = GetMetadataMapFromDefinition(metadataTypeDefinition.RequestDtoType);
            var metadataMapFromMultipartDefinition = GetMetadataMapFromMultipartDefinition(metadataTypeDefinition.RequestDtoType);

            metadataRouteDefinition = EnrichMetadataRouteDefinitionWithParameterDefinitions(metadataTypeDefinition, metadataRouteDefinition);

            yield return new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);
        }

        if (apiEndpointType.IsClosedTypeOf(typeof(ICommandWebApiEndpoint<,>)))
        {
            var apiEndpointInterfaceType = apiEndpointType.GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommandWebApiEndpoint<,>));

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForCommandWithoutRequestWithoutResponse(apiEndpointInterfaceType, apiEndpointType);

            var metadataMapFromDefinition = GetMetadataMapFromDefinition(metadataTypeDefinition.RequestDtoType);
            var metadataMapFromMultipartDefinition = GetMetadataMapFromMultipartDefinition(metadataTypeDefinition.RequestDtoType);

            metadataRouteDefinition = EnrichMetadataRouteDefinitionWithParameterDefinitions(metadataTypeDefinition, metadataRouteDefinition);

            yield return new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);
        }
    }

    private static MetadataRouteDefinition EnrichMetadataRouteDefinitionWithParameterDefinitions(MetadataTypeDefinition metadataTypeDefinition, MetadataRouteDefinition? metadataRouteDefinition)
    {
        MetadataRouteParameterDefinitionType TransformMapFromToParameterDefinitionType(MapFrom mapFrom) =>
            mapFrom switch
            {
                MapFrom.Path   => MetadataRouteParameterDefinitionType.Path,
                MapFrom.Query  => MetadataRouteParameterDefinitionType.Query,
                MapFrom.Header => MetadataRouteParameterDefinitionType.Header,
                MapFrom.Cookie => MetadataRouteParameterDefinitionType.Cookie,
                MapFrom.Form   => MetadataRouteParameterDefinitionType.Form,
                _              => throw new ArgumentOutOfRangeException(nameof(mapFrom), mapFrom, null)
            };

        var routePathParameterDefinitions = WebApiEndpointMetadataTypeService.GetMapFromProperties(metadataTypeDefinition.RequestDtoType)
                                                                             .Select(x => new MetadataRouteParameterDefinition(x.propertyInfo.Name, TransformMapFromToParameterDefinitionType(x.mapFromAttribute.MapFrom), x.propertyInfo.PropertyType))
                                                                             .ToList();

        var allRoutePathParameterDefinitions = metadataRouteDefinition.ParameterDefinitions.OrEmptyIfNull().Concat(routePathParameterDefinitions).ToList();
        
        return metadataRouteDefinition with
        {
            ParameterDefinitions = allRoutePathParameterDefinitions
        };
    }

    private static MetadataMapFromDefinition? GetMetadataMapFromDefinition(Type requestDtoType)
    {
        var parameters = requestDtoType.GetProperties()
                                       .Select(propertyInfo => new { propertyInfo, mapFromAttribute = propertyInfo.GetCustomAttribute<MapFromAttribute>() })
                                       .Where(x => x.mapFromAttribute != null)
                                       .Select(x => new MetadataMapFromParameterDefinition(x.propertyInfo.Name, x.propertyInfo, x.mapFromAttribute))
                                       .ToList();

        return parameters.Any() ? new MetadataMapFromDefinition(parameters) : null;
    }

    private static MetadataMapFromMultipartDefinition? GetMetadataMapFromMultipartDefinition(Type requestDtoType)
    {
        var parameters = requestDtoType.GetProperties()
                                       .Select(propertyInfo => new { propertyInfo, mapFromAttribute = propertyInfo.GetCustomAttribute<MapFromMultipartAttribute>() })
                                       .Where(x => x.mapFromAttribute != null)
                                       .Select(x => new MetadataMapFromMultipartParameterDefinition(x.propertyInfo.Name, x.propertyInfo, x.mapFromAttribute))
                                       .ToList();

        return parameters.Any() ? new MetadataMapFromMultipartDefinition(parameters) : null;
    }
}