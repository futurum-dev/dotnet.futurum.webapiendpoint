using Futurum.Core.Functional;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Internal.Dispatcher;
using Futurum.WebApiEndpoint.Middleware;

namespace Futurum.WebApiEndpoint.Metadata;

internal static class WebApiEndpointMetadataTypeService
{
    public static MetadataTypeDefinition GetForQueryWithoutRequest(Type apiEndpointInterfaceType, Type apiEndpointType)
    {
        var responseDtoType = apiEndpointInterfaceType?.GetGenericArguments()[0];
        var responseType = apiEndpointInterfaceType?.GetGenericArguments()[1];
        var mapperType = apiEndpointInterfaceType?.GetGenericArguments()[2];

        var apiEndpointExecutorServiceType = typeof(QueryWebApiEndpointDispatcher<,,>).MakeGenericType(responseDtoType, responseType, mapperType);

        var middlewareExecutorType = typeof(IWebApiEndpointMiddlewareExecutor<,>).MakeGenericType(typeof(Unit), responseType);

        return new MetadataTypeDefinition(typeof(EmptyRequestDto), responseDtoType, apiEndpointType, apiEndpointInterfaceType, middlewareExecutorType, apiEndpointExecutorServiceType, responseType);
    }
    
    public static MetadataTypeDefinition GetForQueryWithoutRequestDto(Type apiEndpointInterfaceType, Type apiEndpointType)
    {
        var responseDtoType = apiEndpointInterfaceType?.GetGenericArguments()[0];
        var queryType = apiEndpointInterfaceType?.GetGenericArguments()[1];
        var responseType = apiEndpointInterfaceType?.GetGenericArguments()[2];
        var requestMapperType = apiEndpointInterfaceType?.GetGenericArguments()[3];
        var responseMapperType = apiEndpointInterfaceType?.GetGenericArguments()[4];

        var apiEndpointExecutorServiceType = typeof(QueryWebApiEndpointDispatcher<,,,,>).MakeGenericType(responseDtoType, queryType, responseType, requestMapperType, responseMapperType);

        var middlewareExecutorType = typeof(IWebApiEndpointMiddlewareExecutor<,>).MakeGenericType(queryType, responseType);

        return new MetadataTypeDefinition(typeof(EmptyRequestDto), responseDtoType, apiEndpointType, apiEndpointInterfaceType, middlewareExecutorType, apiEndpointExecutorServiceType, responseType);
    }
    
    public static MetadataTypeDefinition GetForQueryWithRequestDto(Type apiEndpointInterfaceType, Type apiEndpointType)
    {
        var requestDtoType = apiEndpointInterfaceType?.GetGenericArguments()[0];
        var responseDtoType = apiEndpointInterfaceType?.GetGenericArguments()[1];
        var queryType = apiEndpointInterfaceType?.GetGenericArguments()[2];
        var responseType = apiEndpointInterfaceType?.GetGenericArguments()[3];
        var requestMapperType = apiEndpointInterfaceType?.GetGenericArguments()[4];
        var responseMapperType = apiEndpointInterfaceType?.GetGenericArguments()[5];

        var apiEndpointExecutorServiceType = typeof(QueryWebApiEndpointDispatcher<,,,,,>).MakeGenericType(requestDtoType, responseDtoType, queryType, responseType, requestMapperType, responseMapperType);

        var middlewareExecutorType = typeof(IWebApiEndpointMiddlewareExecutor<,>).MakeGenericType(queryType, responseType);

        return new MetadataTypeDefinition(requestDtoType, responseDtoType, apiEndpointType, apiEndpointInterfaceType, middlewareExecutorType, apiEndpointExecutorServiceType, responseType);
    }
    
    public static MetadataTypeDefinition GetForCommandWithRequestWithResponse(Type apiEndpointInterfaceType, Type apiEndpointType)
    {
        var commandDtoType = apiEndpointInterfaceType?.GetGenericArguments()[0];
        var responseDtoType = apiEndpointInterfaceType?.GetGenericArguments()[1];
        var commandType = apiEndpointInterfaceType?.GetGenericArguments()[2];
        var responseType = apiEndpointInterfaceType?.GetGenericArguments()[3];
        var requestMapperType = apiEndpointInterfaceType?.GetGenericArguments()[4];
        var responseMapperType = apiEndpointInterfaceType?.GetGenericArguments()[5];

        var apiEndpointExecutorServiceType = typeof(CommandWebApiEndpointDispatcher<,,,,,>).MakeGenericType(commandDtoType, responseDtoType, commandType, responseType, requestMapperType, responseMapperType);

        var middlewareExecutorType = typeof(IWebApiEndpointMiddlewareExecutor<,>).MakeGenericType(commandType, responseType);

        return new MetadataTypeDefinition(commandDtoType, responseDtoType, apiEndpointType, apiEndpointInterfaceType, middlewareExecutorType, apiEndpointExecutorServiceType, responseType);
    }
    
    public static MetadataTypeDefinition GetForCommandWithoutRequestWithResponse(Type apiEndpointInterfaceType, Type apiEndpointType)
    {
        var responseDtoType = apiEndpointInterfaceType?.GetGenericArguments()[0];
        var commandType = apiEndpointInterfaceType?.GetGenericArguments()[1];
        var responseType = apiEndpointInterfaceType?.GetGenericArguments()[2];
        var requestMapperType = apiEndpointInterfaceType?.GetGenericArguments()[3];
        var responseMapperType = apiEndpointInterfaceType?.GetGenericArguments()[4];

        var apiEndpointExecutorServiceType = typeof(CommandWebApiEndpointDispatcher<,,,,>).MakeGenericType(responseDtoType, commandType, responseType, requestMapperType, responseMapperType);

        var middlewareExecutorType = typeof(IWebApiEndpointMiddlewareExecutor<,>).MakeGenericType(commandType, responseType);

        return new MetadataTypeDefinition(typeof(EmptyRequestDto), responseDtoType, apiEndpointType, apiEndpointInterfaceType, middlewareExecutorType, apiEndpointExecutorServiceType, responseType);
    }
    
    public static MetadataTypeDefinition GetForCommandWithoutResponse(Type apiEndpointInterfaceType, Type apiEndpointType)
    {
        var commandDtoType = apiEndpointInterfaceType?.GetGenericArguments()[0];
        var commandType = apiEndpointInterfaceType?.GetGenericArguments()[1];
        var mapperType = apiEndpointInterfaceType?.GetGenericArguments()[2];

        var apiEndpointExecutorServiceType = typeof(CommandWebApiEndpointDispatcher<,,>).MakeGenericType(commandDtoType, commandType, mapperType);

        var middlewareExecutorType = typeof(IWebApiEndpointMiddlewareExecutor<,>).MakeGenericType(commandType, typeof(Unit));

        return new MetadataTypeDefinition(commandDtoType, typeof(EmptyResponseDto), apiEndpointType, apiEndpointInterfaceType, middlewareExecutorType, apiEndpointExecutorServiceType);
    }
    
    public static MetadataTypeDefinition GetForCommandWithoutRequestWithoutResponse(Type apiEndpointInterfaceType, Type apiEndpointType)
    {
        var commandType = apiEndpointInterfaceType?.GetGenericArguments()[0];
        var mapperType = apiEndpointInterfaceType?.GetGenericArguments()[1];

        var apiEndpointExecutorServiceType = typeof(CommandWebApiEndpointDispatcher<,>).MakeGenericType(commandType, mapperType);

        var middlewareExecutorType = typeof(IWebApiEndpointMiddlewareExecutor<,>).MakeGenericType(commandType, typeof(Unit));

        return new MetadataTypeDefinition(typeof(EmptyRequestDto), typeof(EmptyResponseDto), apiEndpointType, apiEndpointInterfaceType, middlewareExecutorType, apiEndpointExecutorServiceType);
    }
}