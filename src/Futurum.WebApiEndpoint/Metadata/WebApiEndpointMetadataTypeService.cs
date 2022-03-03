using System.Reflection;

using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Middleware;

namespace Futurum.WebApiEndpoint.Metadata;

internal static class WebApiEndpointMetadataTypeService
{
    public static MetadataTypeDefinition Get(Type apiEndpointInterfaceType, Type apiEndpointType)
    {
        var requestDtoType = apiEndpointInterfaceType?.GetGenericArguments()[0];

        var underlyingRequestDtoType = requestDtoType;
        if (requestDtoType.IsClosedTypeOf(typeof(IRequestWrapperDto<>)))
        {
            underlyingRequestDtoType = requestDtoType.GetGenericArguments()[0];
        }
        
        var responseDtoType = apiEndpointInterfaceType?.GetGenericArguments()[1];
        
        var underlyingResponseDtoType = responseDtoType;
        if (responseDtoType.IsClosedTypeOf(typeof(IResponseWrapperDto<>)))
        {
            underlyingResponseDtoType = responseDtoType.GetGenericArguments()[0];
        }
        
        var requestType = apiEndpointInterfaceType?.GetGenericArguments()[2];
        var responseType = apiEndpointInterfaceType?.GetGenericArguments()[3];
        var requestMapperType = apiEndpointInterfaceType?.GetGenericArguments()[4];
        var responseMapperType = apiEndpointInterfaceType?.GetGenericArguments()[5];

        var apiEndpointExecutorServiceType = typeof(WebApiEndpointDispatcher<,,,,,>).MakeGenericType(requestDtoType, responseDtoType, requestType, responseType, requestMapperType, responseMapperType);

        var middlewareExecutorType = typeof(IWebApiEndpointMiddlewareExecutor<,>).MakeGenericType(requestType, responseType);

        return new MetadataTypeDefinition(requestDtoType, underlyingRequestDtoType, responseDtoType, underlyingResponseDtoType, apiEndpointType, apiEndpointInterfaceType, middlewareExecutorType, apiEndpointExecutorServiceType,
                                          new[] { requestMapperType, responseMapperType }, responseType);
    }
    
    public static List<(PropertyInfo propertyInfo, MapFromAttribute mapFromAttribute)> GetMapFromProperties(Type requestDto) =>
        requestDto.GetProperties()
                  .Where(propertyInfo => propertyInfo.GetCustomAttribute<MapFromAttribute>() != null)
                  .Select(propertyInfo => (propertyInfo, mapFromAttribute: propertyInfo.GetCustomAttribute<MapFromAttribute>()!))
                  .ToList();

    public static List<(PropertyInfo propertyInfo, MapFromMultipartAttribute mapFromMultipartAttribute)> GetMapFromMultipartProperties(Type requestDto) =>
        requestDto.GetProperties()
                  .Where(propertyInfo => propertyInfo.GetCustomAttribute<MapFromMultipartAttribute>() != null)
                  .Select(propertyInfo => (propertyInfo, mapFromAttribute: propertyInfo.GetCustomAttribute<MapFromMultipartAttribute>()!))
                  .ToList();
}