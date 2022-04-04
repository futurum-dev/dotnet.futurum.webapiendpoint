using System.Reflection;

using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Authorization;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Extension methods for Authorization
/// </summary>
public static class AuthorizationExtensions
{
    /// <summary>
    /// Adds authorization policies to WebApiEndpoint
    /// </summary>
    public static IServiceCollection AddWebApiEndpointAuthorization(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddAuthorization(options =>
        {
            var metadataDefinitions = WebApiEndpointOnApiEndpointDefinitionMetadataProvider.GetMetadata(assemblies);

            foreach (var metadataDefinition in metadataDefinitions)
            {
                metadataDefinition.MetadataRouteDefinition.SecurityDefinition
                                  .DoSwitch(securityDefinition => ConfigurePolicy(options, metadataDefinition, securityDefinition),
                                            Function.DoNothing);
            }
        });

        return services;
    }

    public static string ToAuthorizationPolicy(MetadataDefinition metadataDefinition)
    {
        var apiVersion = metadataDefinition.MetadataRouteDefinition.ApiVersion != null ? $"-{metadataDefinition.MetadataRouteDefinition.ApiVersion.ToString()}" : string.Empty;
        return $"WebApiEndpoint-AuthorizationPolicy" +
               $"-{metadataDefinition.MetadataTypeDefinition.WebApiEndpointType.FullName}" +
               $"-{metadataDefinition.MetadataRouteDefinition.RouteTemplate}" +
               $"-{metadataDefinition.MetadataRouteDefinition.HttpMethod}" +
               $"{apiVersion}";
    }

    private static void ConfigurePolicy(AuthorizationOptions options, MetadataDefinition metadataDefinition, MetadataSecurityDefinition metadataSecurityDefinition)
    {
        options.AddPolicy(ToAuthorizationPolicy(metadataDefinition),
                          builder =>
                          {
                              builder.RequireAuthenticatedUser();

                              ConfigurePermissions(metadataSecurityDefinition, builder);

                              ConfigureClaim(metadataSecurityDefinition, builder);

                              ConfigureRole(metadataSecurityDefinition, builder);
                          });
    }

    private static void ConfigurePermissions(MetadataSecurityDefinition metadataSecurityDefinition, AuthorizationPolicyBuilder builder)
    {
        if (metadataSecurityDefinition.PermissionDefinitions.Any())
        {
            var requiredPermissions = metadataSecurityDefinition.PermissionDefinitions.Select(permissionDefinition => permissionDefinition.Name)
                                                                .ToArray();
            builder.RequireAssertion(authorizationHandlerContext =>
                                         !requiredPermissions.Except(authorizationHandlerContext.User.FindAll(AuthorizationConstants.ClaimType.Permissions).Select(claim => claim.Value)).Any());
        }
    }

    private static void ConfigureClaim(MetadataSecurityDefinition metadataSecurityDefinition, AuthorizationPolicyBuilder builder)
    {
        if (metadataSecurityDefinition.ClaimDefinitions.Any())
        {
            foreach (var claimDefinition in metadataSecurityDefinition.ClaimDefinitions)
            {
                builder.RequireAssertion(claimDefinition.ClaimChecker.Execute);
            }
        }
    }

    private static void ConfigureRole(MetadataSecurityDefinition metadataSecurityDefinition, AuthorizationPolicyBuilder builder)
    {
        if (metadataSecurityDefinition.RoleDefinitions.Any())
        {
            var roles = metadataSecurityDefinition.RoleDefinitions.Select(roleDefinition => roleDefinition.Name).ToArray();

            builder.RequireRole(roles);
        }
    }
}