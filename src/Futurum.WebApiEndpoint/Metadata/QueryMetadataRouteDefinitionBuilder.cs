using System.Collections.Immutable;
using System.Net;

using Futurum.ApiEndpoint.DebugLogger;
using Futurum.Core.Linq;
using Futurum.Core.Option;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Metadata;

/// <summary>
/// Builder for Query <see cref="MetadataRouteDefinition"/>
/// </summary>
public class QueryMetadataRouteDefinitionBuilder : IMetadataRouteDefinitionBuilder
{
    private readonly Type _apiEndpointType;
    private string _route;
    private readonly MetadataRouteHttpMethod _httpMethod = MetadataRouteHttpMethod.Get;
    private readonly List<ApiVersion> _apiVersions = new();
    private readonly List<MetadataRouteParameterDefinition> _parameterDefinitions = new();
    private readonly MetadataRouteOpenApiOperation _openApiOperation = new();
    private int _successStatusCode;
    private int _failedStatusCode;
    private bool _allowFileUploads;
    private Option<Action<RouteHandlerBuilder>> _extendedOptions;

    private bool _allowAnonymous = true;
    private readonly List<MetadataSecurityPermissionDefinition> _permissions = new();
    private readonly List<MetadataSecurityClaimDefinition> _claims = new();
    private readonly List<MetadataSecurityRoleDefinition> _roles = new();

    public QueryMetadataRouteDefinitionBuilder(Type apiEndpointType)
    {
        _apiEndpointType = apiEndpointType;
    }

    /// <summary>
    /// Configure <paramref name="route"/>
    /// </summary>
    public QueryMetadataRouteDefinitionBuilder Route(string route)
    {
        _route = route;

        _successStatusCode = 200;
        _failedStatusCode = 503;

        return this;
    }

    /// <summary>
    /// Configure <paramref name="route"/> with <paramref name="parameterDefinitions"/>
    /// </summary>
    public QueryMetadataRouteDefinitionBuilder Route(string route, params (string name, Type type)[] parameterDefinitions)
    {
        _route = route;

        foreach (var (name, type) in parameterDefinitions)
        {
            _parameterDefinitions.Add(new MetadataRouteParameterDefinition(name, type));
        }

        _successStatusCode = 200;
        _failedStatusCode = 404;

        return this;
    }

    /// <summary>
    /// Configure success <paramref name="httpStatusCode"/>
    /// </summary>
    public QueryMetadataRouteDefinitionBuilder SuccessStatusCode(int httpStatusCode)
    {
        _successStatusCode = httpStatusCode;

        return this;
    }

    /// <summary>
    /// Configure success <paramref name="httpStatusCode"/>
    /// </summary>
    public QueryMetadataRouteDefinitionBuilder SuccessStatusCode(HttpStatusCode httpStatusCode)
    {
        _successStatusCode = (int)httpStatusCode;

        return this;
    }

    /// <summary>
    /// Configure failed <paramref name="httpStatusCode"/>
    /// </summary>
    public QueryMetadataRouteDefinitionBuilder FailedStatusCode(int httpStatusCode)
    {
        _failedStatusCode = httpStatusCode;

        return this;
    }

    /// <summary>
    /// Configure failed <paramref name="httpStatusCode"/>
    /// </summary>
    public QueryMetadataRouteDefinitionBuilder FailedStatusCode(HttpStatusCode httpStatusCode)
    {
        _failedStatusCode = (int)httpStatusCode;

        return this;
    }

    /// <summary>
    /// Configure <paramref name="versions"/>
    /// </summary>
    public QueryMetadataRouteDefinitionBuilder Version(params ApiVersion[] versions)
    {
        _apiVersions.AddRange(versions);

        return this;
    }

    /// <summary>
    /// Configure anonymous authorization.
    /// Remove all other authorizations.
    /// </summary>
    public QueryMetadataRouteDefinitionBuilder AllowAnonymousAuthorization()
    {
        _allowAnonymous = true;
        
        _permissions.Clear();
        _roles.Clear();
        _claims.Clear();

        return this;
    }

    /// <summary>
    /// Configure authorization.
    /// Remove all other authorizations.
    /// </summary>
    public QueryMetadataRouteDefinitionBuilder RequireAuthorization()
    {
        _allowAnonymous = false;
        
        _permissions.Clear();
        _roles.Clear();
        _claims.Clear();

        return this;
    }

    /// <summary>
    /// Configure permission authorizations
    /// </summary>
    public QueryMetadataRouteDefinitionBuilder RequirePermissionAuthorization(params string[] permissions)
    {
        _allowAnonymous = false;

        _permissions.AddRange(permissions.Select(x => new MetadataSecurityPermissionDefinition(x)));

        return this;
    }

    /// <summary>
    /// Configure claim authorizations
    /// </summary>
    public QueryMetadataRouteDefinitionBuilder RequireClaimAuthorization(string type, string name)
    {
        _allowAnonymous = false;

        _claims.Add(new MetadataSecurityClaimDefinition(MetadataSecurityDefinitionHandlers.ClaimEqualityCheck(EnumerableExtensions.Return(new MetadataSecurityDefinitionHandlers.ClaimRecord(type, name)))));

        return this;
    }

    /// <summary>
    /// Configure claim authorizations
    /// </summary>
    public QueryMetadataRouteDefinitionBuilder RequireClaimAuthorization(params (string type, string name)[] claims)
    {
        _allowAnonymous = false;

        _claims.Add(new MetadataSecurityClaimDefinition(MetadataSecurityDefinitionHandlers.ClaimEqualityCheck(claims.Select(x => new MetadataSecurityDefinitionHandlers.ClaimRecord(x.type, x.name)))));

        return this;
    }

    /// <summary>
    /// Configure claim authorizations
    /// </summary>
    public QueryMetadataRouteDefinitionBuilder RequireClaimAuthorization(Func<AuthorizationHandlerContext, bool> handler)
    {
        _allowAnonymous = false;

        _claims.Add(new MetadataSecurityClaimDefinition(handler));

        return this;
    }

    /// <summary>
    /// Configure role authorizations
    /// </summary>
    public QueryMetadataRouteDefinitionBuilder RequireRoleAuthorization(params string[] roles)
    {
        _allowAnonymous = false;

        _roles.AddRange(roles.Select(x => new MetadataSecurityRoleDefinition(x)));

        return this;
    }

    /// <summary>
    /// Configure OpenApi <paramref name="summary"/>
    /// </summary>
    public QueryMetadataRouteDefinitionBuilder Summary(string summary)
    {
        _openApiOperation.Summary = summary;

        return this;
    }

    /// <summary>
    /// Configure OpenApi <paramref name="description"/>
    /// </summary>
    public QueryMetadataRouteDefinitionBuilder Description(string description)
    {
        _openApiOperation.Description = description;

        return this;
    }

    /// <summary>
    /// Configure allow file uploads
    /// </summary>
    public QueryMetadataRouteDefinitionBuilder AllowFileUploads()
    {
        _allowFileUploads = true;

        return this;
    }

    /// <summary>
    /// Configure escape hatch
    /// </summary>
    public QueryMetadataRouteDefinitionBuilder ExtendedOptions(Action<RouteHandlerBuilder> builder)
    {
        _extendedOptions = builder;

        return this;
    }

    IEnumerable<MetadataRouteDefinition> IMetadataRouteDefinitionBuilder.Build()
    {
        if (_apiVersions.Any())
        {
            var metadataRouteDefinitions =
                _apiVersions.Select(apiVersion =>
                {
                    var metadataSecurityDefinition = CreateMetadataSecurityDefinition();
                    return new MetadataRouteDefinition(_httpMethod, _route, apiVersion, _parameterDefinitions, _openApiOperation, _successStatusCode, _failedStatusCode,
                                                       _allowFileUploads, _extendedOptions, metadataSecurityDefinition);
                });

            foreach (var metadataRouteDefinition in metadataRouteDefinitions)
            {
                yield return metadataRouteDefinition;
            }
        }
        else
        {
            var metadataSecurityDefinition = CreateMetadataSecurityDefinition();
            yield return new MetadataRouteDefinition(_httpMethod, _route, null, _parameterDefinitions, _openApiOperation, _successStatusCode, _failedStatusCode, _allowFileUploads, _extendedOptions,
                                                     metadataSecurityDefinition);
        }
    }

    ApiEndpointDebugNode IMetadataRouteDefinitionBuilder.Debug()
    {
        var apiEndpointDebugNode = new ApiEndpointDebugNode
        {
            Name = $"{_route}-{_httpMethod} ({_apiEndpointType.FullName})"
        };

        if (_apiVersions.Any())
        {
            apiEndpointDebugNode.Children = _apiVersions.Select(apiVersion => new ApiEndpointDebugNode { Name = apiVersion.ToString() }).ToList();
        }

        return apiEndpointDebugNode;
    }

    private Option<MetadataSecurityDefinition> CreateMetadataSecurityDefinition()
    {
        if (_allowAnonymous)
        {
            return Option<MetadataSecurityDefinition>.None;
        }

        return new MetadataSecurityDefinition(_permissions.ToImmutableList(), _roles.ToImmutableList(), _claims.ToImmutableList());
    }
}