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
        var metadataDefinitions = WebApiEndpointOnApiEndpointDefinitionMetadataProvider.GetMetadata(_assemblies);

        RegisterWebApiEndpoints(services, metadataDefinitions);

        RegisterMappers(services, metadataDefinitions, _assemblies);

        RegisterOpenApiRequestConfigurations(services, new []{typeof(WebApiEndpointAssemblyHook).Assembly});
        RegisterOpenApiRequestConfigurations(services, _assemblies);
        
        RegisterOpenApiResponseConfigurations(services, new []{typeof(WebApiEndpointAssemblyHook).Assembly});
        RegisterOpenApiResponseConfigurations(services, _assemblies);
    }

    private static void RegisterWebApiEndpoints(IServiceCollection services, IEnumerable<MetadataDefinition> metadataDefinitions)
    {
        services.Scan(scan => scan.AddTypes(metadataDefinitions.Select(metadataDefinition => metadataDefinition.MetadataTypeDefinition)
                                                               .Select(metadataTypeDefinition => metadataTypeDefinition.WebApiEndpointType))
                                  .AsImplementedInterfaces()
                                  .WithScopedLifetime());
    }

    private static void RegisterMappers(IServiceCollection services, IEnumerable<MetadataDefinition> metadataDefinitions, IEnumerable<Assembly> assemblies)
    {
        var assembliesIncludingLibrary = assemblies.Concat(new[] { typeof(WebApiEndpointAssemblyHook).Assembly }).ToArray();
        RegisterMappersOfType(services, assembliesIncludingLibrary, typeof(IWebApiEndpointRequestMapper<>));
        RegisterMappersOfType(services, assembliesIncludingLibrary, typeof(IWebApiEndpointRequestMapper<,>));
        RegisterMappersOfType(services, assembliesIncludingLibrary, typeof(IWebApiEndpointResponseMapper<>));
        RegisterMappersOfType(services, assembliesIncludingLibrary, typeof(IWebApiEndpointResponseMapper<,>));
        RegisterMappersOfType(services, assembliesIncludingLibrary, typeof(IWebApiEndpointResponseDataMapper<,>));
        RegisterMappersOfType(services, assembliesIncludingLibrary, typeof(IWebApiEndpointRequestPayloadMapper<,>));
        RegisterMappersOfType(services, assembliesIncludingLibrary, typeof(IWebApiEndpointResponseDtoMapper<,>));

        foreach (var mapperType in metadataDefinitions.Select(metadataDefinition => metadataDefinition.MetadataTypeDefinition)
                                                      .SelectMany(metadataTypeDefinition => metadataTypeDefinition.MapperTypes))
        {
            services.AddSingleton(mapperType);
        }

        services.AddSingleton(typeof(IRequestJsonReader<>), typeof(RequestJsonReader<>));
    }

    private static void RegisterMappersOfType(IServiceCollection services, Assembly[] assemblies, Type apiEndpointMapperType)
    {
        services.Scan(scan => scan.FromAssemblies(assemblies)
                                  .AddClasses(classes => classes.Where(type => type.IsClosedTypeOf(apiEndpointMapperType)))
                                  .AsSelfWithInterfaces()
                                  .WithSingletonLifetime());
    }

    private static void RegisterOpenApiRequestConfigurations(IServiceCollection services, Assembly[] assemblies)
    {
        services.Scan(scan => scan.FromAssemblies(assemblies)
                                  .AddClasses(classes => classes.Where(type => type.GetInterface(nameof(IWebApiOpenApiRequestConfiguration)) != null))
                                  .AsImplementedInterfaces()
                                  .WithSingletonLifetime());
    }

    private static void RegisterOpenApiResponseConfigurations(IServiceCollection services, Assembly[] assemblies)
    {
        services.Scan(scan => scan.FromAssemblies(assemblies)
                                  .AddClasses(classes => classes.Where(type => type.GetInterface(nameof(IWebApiOpenApiResponseConfiguration)) != null))
                                  .AsImplementedInterfaces()
                                  .WithSingletonLifetime());
    }
}