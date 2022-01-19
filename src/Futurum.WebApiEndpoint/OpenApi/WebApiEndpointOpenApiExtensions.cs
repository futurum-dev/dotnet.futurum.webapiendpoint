using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Futurum.WebApiEndpoint.OpenApi;

/// <summary>
/// Extension methods for making OpenApi with <see cref="WebApiEndpoint"/>
/// </summary>
public static class WebApiEndpointOpenApiExtensions
{
    /// <summary>
    /// Configures OpenApi to work with <see cref="WebApiEndpoint"/>
    /// </summary>
    public static SwaggerGenOptions EnableWebApiEndpointForOpenApi(this SwaggerGenOptions options, string documentTitle, params ApiVersion[] apiVersions)
    {
        options.CustomSchemaIds(type => type.FullName);
        options.OperationFilter<WebApiEndpointOpenApiOperationFilter>();

        foreach (var apiVersion in apiVersions)
        {
            options.SwaggerDoc($"v{apiVersion}", new OpenApiInfo { Title = documentTitle, Version = $"v{apiVersion}" });
        }

        return options;
    }

    /// <summary>
    /// Configures Jwt for OpenApi to work with <see cref="WebApiEndpoint"/>
    /// </summary>
    public static SwaggerGenOptions EnableWebApiEndpointJwtForOpenApi(this SwaggerGenOptions options)
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

        return options;
    }

    /// <summary>
    /// Configures OpenApi UI to work with <see cref="WebApiEndpoint"/>
    /// </summary>
    public static SwaggerUIOptions UseWebApiEndpointOpenApiUI(this SwaggerUIOptions options, string documentTitle, params ApiVersion[] apiVersions)
    {
        foreach (var apiVersion in apiVersions)
        {
            options.SwaggerEndpoint($"v{apiVersion}/swagger.json", $"{documentTitle} V{apiVersion}");
        }

        return options;
    }
}