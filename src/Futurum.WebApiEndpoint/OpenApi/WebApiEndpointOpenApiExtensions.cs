using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace Futurum.WebApiEndpoint.OpenApi;

/// <summary>
/// Extension methods for making OpenApi with <see cref="WebApiEndpoint"/>
/// </summary>
public static class WebApiEndpointOpenApiExtensions
{
    /// <summary>
    /// Configures OpenApi to work with <see cref="WebApiEndpoint"/>
    /// </summary>
    public static IServiceCollection EnableOpenApiForWebApiEndpoint(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddEndpointsApiExplorer();
        
        serviceCollection.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.FullName?.Replace("+", "_"));
            options.OperationFilter<WebApiEndpointOpenApiOperationRequestBodyRequired>();
            options.OperationFilter<WebApiEndpointOpenApiOperationTypeInformation>();
            options.OperationFilter<WebApiEndpointOpenApiOperationInformation>();
            options.OperationFilter<WebApiEndpointOpenApiOperationDeprecated>();
        });

        return serviceCollection;
    }

    /// <summary>
    /// Configures Jwt for OpenApi to work with <see cref="WebApiEndpoint"/>
    /// </summary>
    public static IServiceCollection EnableOpenApiJwtForWebApiEndpoint(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(options =>
        {
            var securityScheme = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JSON Web Token based security",
            };

            var securityReq = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            };

            options.AddSecurityDefinition("Bearer", securityScheme);
            options.AddSecurityRequirement(securityReq);
        });

        return serviceCollection;
    }

    /// <summary>
    /// Add an OpenApi Version to <see cref="WebApiEndpoint"/>
    /// </summary>
    public static IServiceCollection AddOpenApiVersion(this IServiceCollection serviceCollection, string title, ApiVersion apiVersion, bool deprecated = false)
    {
        serviceCollection.AddSwaggerGen(options => options.SwaggerDoc($"v{apiVersion}", new OpenApiInfo { Title = title, Version = $"v{apiVersion}" }));

        serviceCollection.AddSingleton(new WebApiEndpointOpenApiVersion(title, apiVersion, deprecated));

        return serviceCollection;
    }

    /// <summary>
    /// Configures OpenApi UI to work with <see cref="WebApiEndpoint"/>
    /// </summary>
    public static WebApplication UseOpenApiUIForWebApiEndpoint(this WebApplication application)
    {
        application.UseSwagger();

        var openApiVersions = application.Services.GetServices<WebApiEndpointOpenApiVersion>();
        
        application.UseSwaggerUI(options =>
        {
            foreach (var (title, apiVersion, _) in openApiVersions)
            {
                options.SwaggerEndpoint($"v{apiVersion}/swagger.json", $"{title} V{apiVersion}");
            }
        });

        return application;
    }
}