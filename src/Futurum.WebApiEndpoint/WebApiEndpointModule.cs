using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using FluentValidation;

using Futurum.ApiEndpoint;
using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

namespace Futurum.WebApiEndpoint;

[ExcludeFromCodeCoverage]
public class WebApiEndpointModule : IModule
{
    private readonly WebApiEndpointConfiguration _configuration;
    private readonly Assembly[] _assemblies;

    public WebApiEndpointModule(WebApiEndpointConfiguration configuration, params Assembly[] assemblies)
    {
        _configuration = configuration;
        _assemblies = assemblies;
    }

    public WebApiEndpointModule(params Assembly[] assemblies)
        : this(WebApiEndpointConfiguration.Default, assemblies)
    {
    }

    public void Load(IServiceCollection services)
    {
        services.AddSingleton<IWebApiEndpointAssemblyStore>(new WebApiEndpointAssemblyStore(_assemblies));
        
        services.RegisterModule(new ApiEndpointModule(_assemblies));

        services.RegisterModule(new WebApiEndpointAndMapperModule(_assemblies));

        RegisterConfiguration(services, _configuration);

        RegisterLogger(services);

        RegisterHttpContextDispatcher(services);

        RegisterDispatcher(services);

        RegisterValidation(services, _assemblies);

        RegisterMetadata(services);

        RegisterOpenApi(services);

        RegisterMiddleware(services, _assemblies, _configuration);
    }

    private static void RegisterConfiguration(IServiceCollection services, WebApiEndpointConfiguration configuration)
    {
        services.AddSingleton(configuration);
    }

    private static void RegisterLogger(IServiceCollection services)
    {
        services.AddSingleton<IWebApiEndpointLogger, WebApiEndpointLogger>();
        services.AddSingleton<IApiEndpointLogger, WebApiEndpointLogger>();
    }

    private static void RegisterHttpContextDispatcher(IServiceCollection services)
    {
        services.AddSingleton<IWebApiEndpointHttpContextDispatcher, WebApiEndpointHttpContextDispatcher>();
    }

    private static void RegisterDispatcher(IServiceCollection services)
    {
        services.AddSingleton(typeof(WebApiEndpointDispatcher<,,,,,>));
    }

    private static void RegisterValidation(IServiceCollection services, Assembly[] assemblies)
    {
        services.AddSingleton(typeof(IWebApiEndpointRequestValidation<>), typeof(WebApiEndpointRequestValidation<>));

        services.Scan(scan => scan.FromAssemblies(assemblies)
                                  .AddClasses(classes => classes.Where(type => type.IsClosedTypeOf(typeof(IValidator<>))))
                                  .AsImplementedInterfaces()
                                  .WithSingletonLifetime());
    }

    private static void RegisterMetadata(IServiceCollection services)
    {
        services.AddSingleton<IWebApiEndpointMetadataCache, WebApiEndpointMetadataCache>();
    }

    private static void RegisterOpenApi(IServiceCollection services)
    {
        services.AddSingleton<IRequestOpenApiTypeCreator, RequestOpenApiTypeCreator>();
        services.AddSingleton<IEndpointRouteOpenApiBuilder, EndpointRouteOpenApiBuilder>();
        services.AddSingleton<IEndpointRouteOpenApiBuilderApiVersion, EndpointRouteOpenApiBuilderApiVersion>();
        services.AddSingleton<IEndpointRouteOpenApiBuilderTag, EndpointRouteOpenApiBuilderTag>();
        services.AddSingleton<IEndpointRouteSecurityBuilder, EndpointRouteSecurityBuilder>();
    }

    private static void RegisterMiddleware(IServiceCollection services, Assembly[] assemblies, WebApiEndpointConfiguration configuration)
    {
        if (configuration.EnableMiddleware)
        {
            RegisterEnabledMiddleware(services, assemblies);
        }
        else
        {
            RegisterDisabledMiddleware(services);
        }
    }

    private static void RegisterDisabledMiddleware(IServiceCollection services)
    {
        services.AddSingleton(typeof(IWebApiEndpointMiddlewareExecutor<,>), typeof(DisabledWebApiEndpointMiddlewareExecutor<,>));
    }

    private static void RegisterEnabledMiddleware(IServiceCollection services, Assembly[] assemblies)
    {
        services.AddSingleton(typeof(IWebApiEndpointMiddlewareExecutor<,>), typeof(WebApiEndpointMiddlewareExecutor<,>));
        services.AddSingleton(typeof(IWebApiEndpointMiddleware<,>), typeof(WebApiEndpointPreProcessorMiddleware<,>));
        services.AddSingleton(typeof(IWebApiEndpointMiddleware<,>), typeof(WebApiEndpointPostProcessorMiddleware<,>));

        services.Scan(scan => scan.FromAssemblies(assemblies)
                                  .AddClasses(classes => classes.Where(type => type.IsClosedTypeOf(typeof(IWebApiEndpointPostProcessorMiddleware<,>))))
                                  .AsImplementedInterfaces()
                                  .WithSingletonLifetime());

        services.Scan(scan => scan.FromAssemblies(assemblies)
                                  .AddClasses(classes => classes.Where(type => type.IsClosedTypeOf(typeof(IWebApiEndpointPreProcessorMiddleware<>))))
                                  .AsImplementedInterfaces()
                                  .WithSingletonLifetime());
    }
}