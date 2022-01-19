using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

[ExcludeFromCodeCoverage]
public class WebApiEndpointAndMapperModule : IModule
{
    private readonly Assembly[] _assemblies;

    public WebApiEndpointAndMapperModule(params Assembly[] assemblies)
    {
        _assemblies = assemblies;
    }

    public void Load(IServiceCollection services)
    {
        RegisterQueryWithoutRequestWebApiEndpoints(services, _assemblies);

        RegisterQueryWithoutRequestDtoWebApiEndpoints(services, _assemblies);

        RegisterQueryWithRequestDtoWebApiEndpoints(services, _assemblies);

        RegisterCommandWithResponseWebApiEndpoints(services, _assemblies);

        RegisterCommandWithoutRequestWithResponseWebApiEndpoints(services, _assemblies);

        RegisterCommandWithoutResponseWebApiEndpoints(services, _assemblies);

        RegisterCommandWithoutRequestWithoutResponseWebApiEndpoints(services, _assemblies);

        RegisterMappers(services, _assemblies);
    }

    private static void RegisterQueryWithoutRequestWebApiEndpoints(IServiceCollection services, Assembly[] assemblies)
    {
        RegisterApiEndpoints(services, assemblies, typeof(IQueryWebApiEndpoint<,>));
    }

    private static void RegisterQueryWithoutRequestDtoWebApiEndpoints(IServiceCollection services, Assembly[] assemblies)
    {
        RegisterApiEndpoints(services, assemblies, typeof(IQueryWebApiEndpoint<,,>));
    }

    private static void RegisterQueryWithRequestDtoWebApiEndpoints(IServiceCollection services, Assembly[] assemblies)
    {
        RegisterApiEndpoints(services, assemblies, typeof(IQueryWebApiEndpoint<,,,>));
    }

    private static void RegisterCommandWithResponseWebApiEndpoints(IServiceCollection services, Assembly[] assemblies)
    {
        RegisterApiEndpoints(services, assemblies, typeof(ICommandWebApiEndpoint<,,,>));
    }

    private static void RegisterCommandWithoutRequestWithResponseWebApiEndpoints(IServiceCollection services, Assembly[] assemblies)
    {
        RegisterApiEndpoints(services, assemblies, typeof(ICommandWebApiEndpoint<,,>));
    }

    private static void RegisterCommandWithoutResponseWebApiEndpoints(IServiceCollection services, Assembly[] assemblies)
    {
        RegisterApiEndpoints(services, assemblies, typeof(ICommandWebApiEndpoint<,>));
    }

    private static void RegisterCommandWithoutRequestWithoutResponseWebApiEndpoints(IServiceCollection services, Assembly[] assemblies)
    {
        RegisterApiEndpoints(services, assemblies, typeof(ICommandWebApiEndpoint<>));
    }

    private static void RegisterMappers(IServiceCollection services, Assembly[] assemblies)
    {
        var assembliesIncludingLibrary = assemblies.Concat(new[] { typeof(WebApiEndpointAssemblyHook).Assembly }).ToArray();
        RegisterMapper(services, assembliesIncludingLibrary, typeof(IWebApiEndpointRequestMapper<>));
        RegisterMapper(services, assembliesIncludingLibrary, typeof(IWebApiEndpointRequestMapper<,>));
        RegisterMapper(services, assembliesIncludingLibrary, typeof(IWebApiEndpointResponseMapper<,>));
        RegisterMapper(services, assembliesIncludingLibrary, typeof(IWebApiEndpointDataMapper<,>));

        RegisterRequestPlainTextMapper(services, assemblies);

        RegisterResponseAsyncEnumerableMapper(services, assemblies);

        RegisterResponseBytesMapper(services, assemblies);

        RegisterResponseDataCollectionMapper(services, assemblies);

        RegisterResponseEmptyJsonMapper(services, assemblies);

        RegisterResponseFileStreamMapper(services, assemblies);

        RegisterResponseStreamMapper(services, assemblies);
    }

    private static void RegisterMapper(IServiceCollection services, Assembly[] assemblies, Type apiEndpointMapperType)
    {
        services.Scan(scan => scan.FromAssemblies(assemblies)
                                  .AddClasses(classes => classes.Where(type =>
                                  {
                                      var isClosedTypeOf = type.IsClosedTypeOf(apiEndpointMapperType);

                                      if (isClosedTypeOf)
                                      {
                                          var x = 2;
                                      }
                                      
                                      return isClosedTypeOf;
                                  }))
                                  .AsImplementedInterfaces()
                                  .WithSingletonLifetime());
    }

    private static void RegisterApiEndpoints(IServiceCollection services, Assembly[] assemblies, Type apiEndpointType)
    {
        services.Scan(scan => scan.FromAssemblies(assemblies)
                                  .AddClasses(classes => classes.Where(type => type.IsClosedTypeOf(apiEndpointType)))
                                  .AsImplementedInterfaces()
                                  .WithSingletonLifetime());
    }

    private static void RegisterRequestPlainTextMapper(IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        var metadataTypeDefinitions = assemblies.SelectMany(assembly => assembly.GetTypes())
                                                .SelectMany(GetApiEndpointMetadataTypeDefinitions)
                                                .Where(metadataTypeDefinition => metadataTypeDefinition.RequestDtoType == typeof(RequestPlainTextDto));

        foreach (var metadataTypeDefinition in metadataTypeDefinitions)
        {
            var requestPlainTextMapperType = typeof(RequestPlainTextMapper<>).MakeGenericType(metadataTypeDefinition.WebApiEndpointType);

            var requestPlainTextType = typeof(RequestPlainText<>).MakeGenericType(metadataTypeDefinition.WebApiEndpointType);

            var requestPlainTextMapperInterfaceType = typeof(IWebApiEndpointRequestMapper<,>).MakeGenericType(typeof(RequestPlainTextDto), requestPlainTextType);

            services.AddSingleton(requestPlainTextMapperInterfaceType, requestPlainTextMapperType);
        }
    }

    private static void RegisterResponseAsyncEnumerableMapper(IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        var metadataTypeDefinitions = assemblies.SelectMany(assembly => assembly.GetTypes())
                                                .SelectMany(GetApiEndpointMetadataTypeDefinitions)
                                                .Where(metadataTypeDefinition => metadataTypeDefinition.ResponseDtoType.IsGenericType &&
                                                                                 metadataTypeDefinition.ResponseDtoType.GetGenericTypeDefinition() == typeof(ResponseAsyncEnumerableDto<>));

        foreach (var metadataTypeDefinition in metadataTypeDefinitions)
        {
            var dataDtoType = metadataTypeDefinition.ResponseDtoType.GetGenericArguments()[0];
            var dataType = metadataTypeDefinition.ResponseType.GetGenericArguments()[1];

            var mapperType = typeof(ResponseAsyncEnumerableMapper<,,>).MakeGenericType(metadataTypeDefinition.WebApiEndpointType, dataType, dataDtoType);

            var responseDomain = typeof(ResponseAsyncEnumerable<,>).MakeGenericType(metadataTypeDefinition.WebApiEndpointType, dataType);

            var mapperInterfaceType = typeof(IWebApiEndpointResponseMapper<,>).MakeGenericType(responseDomain, metadataTypeDefinition.ResponseDtoType);

            services.AddSingleton(mapperInterfaceType, mapperType);
        }
    }

    private static void RegisterResponseBytesMapper(IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        var metadataTypeDefinitions = assemblies.SelectMany(assembly => assembly.GetTypes())
                                                .SelectMany(GetApiEndpointMetadataTypeDefinitions)
                                                .Where(metadataTypeDefinition => metadataTypeDefinition.ResponseDtoType == typeof(ResponseBytesDto));

        foreach (var metadataTypeDefinition in metadataTypeDefinitions)
        {
            var mapperType = typeof(ResponseBytesMapper<>).MakeGenericType(metadataTypeDefinition.WebApiEndpointType);

            var responseDomain = typeof(ResponseBytes<>).MakeGenericType(metadataTypeDefinition.WebApiEndpointType);

            var mapperInterfaceType = typeof(IWebApiEndpointResponseMapper<,>).MakeGenericType(responseDomain, typeof(ResponseBytesDto));

            services.AddSingleton(mapperInterfaceType, mapperType);
        }
    }

    private static void RegisterResponseDataCollectionMapper(IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        var metadataTypeDefinitions = assemblies.SelectMany(assembly => assembly.GetTypes())
                                                .SelectMany(GetApiEndpointMetadataTypeDefinitions)
                                                .Where(metadataTypeDefinition => metadataTypeDefinition.ResponseDtoType.IsGenericType &&
                                                                                 metadataTypeDefinition.ResponseDtoType.GetGenericTypeDefinition() == typeof(ResponseDataCollectionDto<>));

        foreach (var metadataTypeDefinition in metadataTypeDefinitions)
        {
            var dataDtoType = metadataTypeDefinition.ResponseDtoType.GetGenericArguments()[0];
            var dataType = metadataTypeDefinition.ResponseType.GetGenericArguments()[1];

            var mapperType = typeof(ResponseDataCollectionMapper<,,>).MakeGenericType(metadataTypeDefinition.WebApiEndpointType, dataType, dataDtoType);

            var responseDomain = typeof(ResponseDataCollection<,>).MakeGenericType(metadataTypeDefinition.WebApiEndpointType, dataType);

            var mapperInterfaceType = typeof(IWebApiEndpointResponseMapper<,>).MakeGenericType(responseDomain, metadataTypeDefinition.ResponseDtoType);

            services.AddSingleton(mapperInterfaceType, mapperType);
        }
    }

    private static void RegisterResponseEmptyJsonMapper(IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        var metadataTypeDefinitions = assemblies.SelectMany(assembly => assembly.GetTypes())
                                                .SelectMany(GetApiEndpointMetadataTypeDefinitions)
                                                .Where(metadataTypeDefinition => metadataTypeDefinition.ResponseDtoType == typeof(ResponseEmptyJsonDto));

        foreach (var metadataTypeDefinition in metadataTypeDefinitions)
        {
            var mapperType = typeof(ResponseEmptyJsonMapper<>).MakeGenericType(metadataTypeDefinition.WebApiEndpointType);

            var responseDomain = typeof(ResponseEmptyJson<>).MakeGenericType(metadataTypeDefinition.WebApiEndpointType);

            var mapperInterfaceType = typeof(IWebApiEndpointResponseMapper<,>).MakeGenericType(responseDomain, typeof(ResponseEmptyJsonDto));

            services.AddSingleton(mapperInterfaceType, mapperType);
        }
    }

    private static void RegisterResponseFileStreamMapper(IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        var metadataTypeDefinitions = assemblies.SelectMany(assembly => assembly.GetTypes())
                                                .SelectMany(GetApiEndpointMetadataTypeDefinitions)
                                                .Where(metadataTypeDefinition => metadataTypeDefinition.ResponseDtoType == typeof(ResponseFileStreamDto));

        foreach (var metadataTypeDefinition in metadataTypeDefinitions)
        {
            var mapperType = typeof(ResponseFileStreamMapper<>).MakeGenericType(metadataTypeDefinition.WebApiEndpointType);

            var responseDomain = typeof(ResponseFileStream<>).MakeGenericType(metadataTypeDefinition.WebApiEndpointType);

            var mapperInterfaceType = typeof(IWebApiEndpointResponseMapper<,>).MakeGenericType(responseDomain, typeof(ResponseFileStreamDto));

            services.AddSingleton(mapperInterfaceType, mapperType);
        }
    }

    private static void RegisterResponseStreamMapper(IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        var metadataTypeDefinitions = assemblies.SelectMany(assembly => assembly.GetTypes())
                                                .SelectMany(GetApiEndpointMetadataTypeDefinitions)
                                                .Where(metadataTypeDefinition => metadataTypeDefinition.ResponseDtoType == typeof(ResponseStreamDto));

        foreach (var metadataTypeDefinition in metadataTypeDefinitions)
        {
            var mapperType = typeof(ResponseStreamMapper<>).MakeGenericType(metadataTypeDefinition.WebApiEndpointType);

            var responseDomain = typeof(ResponseStream<>).MakeGenericType(metadataTypeDefinition.WebApiEndpointType);

            var mapperInterfaceType = typeof(IWebApiEndpointResponseMapper<,>).MakeGenericType(responseDomain, typeof(ResponseStreamDto));

            services.AddSingleton(mapperInterfaceType, mapperType);
        }
    }

    private static IEnumerable<MetadataTypeDefinition> GetApiEndpointMetadataTypeDefinitions(Type type)
    {
        if (type.IsClosedTypeOf(typeof(IQueryWebApiEndpoint<,>)))
        {
            var apiEndpointInterfaceType = type.GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IQueryWebApiEndpoint<,>));

            yield return WebApiEndpointMetadataTypeService.GetForQueryWithoutRequest(apiEndpointInterfaceType, type);
        }

        if (type.IsClosedTypeOf(typeof(IQueryWebApiEndpoint<,,>)))
        {
            var apiEndpointInterfaceType = type.GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IQueryWebApiEndpoint<,,>));

            yield return WebApiEndpointMetadataTypeService.GetForQueryWithoutRequestDto(apiEndpointInterfaceType, type);
        }

        if (type.IsClosedTypeOf(typeof(IQueryWebApiEndpoint<,,,>)))
        {
            var apiEndpointInterfaceType = type.GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IQueryWebApiEndpoint<,,,>));

            yield return WebApiEndpointMetadataTypeService.GetForQueryWithRequestDto(apiEndpointInterfaceType, type);
        }

        if (type.IsClosedTypeOf(typeof(ICommandWebApiEndpoint<,,,>)))
        {
            var apiEndpointInterfaceType = type.GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommandWebApiEndpoint<,,,>));

            yield return WebApiEndpointMetadataTypeService.GetForCommandWithRequestWithResponse(apiEndpointInterfaceType, type);
        }

        if (type.IsClosedTypeOf(typeof(ICommandWebApiEndpoint<,,>)))
        {
            var apiEndpointInterfaceType = type.GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommandWebApiEndpoint<,,>));

            yield return WebApiEndpointMetadataTypeService.GetForCommandWithoutRequestWithResponse(apiEndpointInterfaceType, type);
        }

        if (type.IsClosedTypeOf(typeof(ICommandWebApiEndpoint<,>)))
        {
            var apiEndpointInterfaceType = type.GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommandWebApiEndpoint<,>));

            yield return WebApiEndpointMetadataTypeService.GetForCommandWithoutResponse(apiEndpointInterfaceType, type);
        }

        if (type.IsClosedTypeOf(typeof(ICommandWebApiEndpoint<>)))
        {
            var apiEndpointInterfaceType = type.GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommandWebApiEndpoint<>));

            yield return WebApiEndpointMetadataTypeService.GetForCommandWithoutRequestWithoutResponse(apiEndpointInterfaceType, type);
        }
    }
}