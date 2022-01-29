using Futurum.Core.Option;
using Futurum.WebApiEndpoint.Internal.Authorization;

using Microsoft.AspNetCore.Authorization;

namespace Futurum.WebApiEndpoint.Metadata;

public partial class MetadataRouteDefinitionBuilder
{
    /// <summary>
    /// Configure anonymous authorization.
    /// <remarks>
    /// Removes all other authorizations
    /// </remarks>
    /// </summary>
    public MetadataRouteDefinitionBuilder AllowAnonymousAuthorization()
    {
        _metadataRouteDefinition = _metadataRouteDefinition with
        {
            SecurityDefinition = Option<MetadataSecurityDefinition>.None
        };

        return this;
    }

    /// <summary>
    /// Configure authorization.
    /// <remarks>
    /// Removes all other authorizations
    /// </remarks>
    /// </summary>
    public MetadataRouteDefinitionBuilder RequireAuthorization()
    {
        _metadataRouteDefinition = _metadataRouteDefinition with
        {
            SecurityDefinition = new MetadataSecurityDefinition(new(),
                                                                new(),
                                                                new())
        };

        return this;
    }

    /// <summary>
    /// Configure permission authorizations
    /// </summary>
    public MetadataRouteDefinitionBuilder RequirePermissionAuthorization(params string[] permissions)
    {
        void HasValue(MetadataSecurityDefinition securityDefinition)
        {
            securityDefinition.PermissionDefinitions.AddRange(permissions.Select(x => new MetadataSecurityPermissionDefinition(x)));

            _metadataRouteDefinition = _metadataRouteDefinition with
            {
                SecurityDefinition = securityDefinition
            };
        }

        void HasNoValue()
        {
            var securityDefinition = new MetadataSecurityDefinition(new(),
                                                                    new(),
                                                                    new());

            HasValue(securityDefinition);
        }

        _metadataRouteDefinition.SecurityDefinition.DoSwitch(HasValue, HasNoValue);

        return this;
    }

    /// <summary>
    /// Configure claim authorizations
    /// </summary>
    public MetadataRouteDefinitionBuilder RequireClaimAuthorization(string type, string name)
    {
        void HasValue(MetadataSecurityDefinition securityDefinition)
        {
            securityDefinition.ClaimDefinitions.Add(new MetadataSecurityClaimDefinition(ClaimCheckerFactory.CreateEqualityChecker(new ClaimRecord(type, name))));

            _metadataRouteDefinition = _metadataRouteDefinition with
            {
                SecurityDefinition = securityDefinition
            };
        }

        void HasNoValue()
        {
            var securityDefinition = new MetadataSecurityDefinition(new(),
                                                                    new(),
                                                                    new());

            HasValue(securityDefinition);
        }

        _metadataRouteDefinition.SecurityDefinition.DoSwitch(HasValue, HasNoValue);

        return this;
    }

    /// <summary>
    /// Configure claim authorizations
    /// </summary>
    public MetadataRouteDefinitionBuilder RequireClaimAuthorization(params (string type, string name)[] claims)
    {
        void HasValue(MetadataSecurityDefinition securityDefinition)
        {
            securityDefinition.ClaimDefinitions.Add(new MetadataSecurityClaimDefinition(ClaimCheckerFactory.CreateEqualityChecker(claims.Select(x => new ClaimRecord(x.type, x.name)))));

            _metadataRouteDefinition = _metadataRouteDefinition with
            {
                SecurityDefinition = securityDefinition
            };
        }

        void HasNoValue()
        {
            var securityDefinition = new MetadataSecurityDefinition(new(),
                                                                    new(),
                                                                    new());

            HasValue(securityDefinition);
        }

        _metadataRouteDefinition.SecurityDefinition.DoSwitch(HasValue, HasNoValue);

        return this;
    }

    /// <summary>
    /// Configure claim authorizations
    /// </summary>
    public MetadataRouteDefinitionBuilder RequireClaimAuthorization(Func<AuthorizationHandlerContext, bool> handler)
    {
        void HasValue(MetadataSecurityDefinition securityDefinition)
        {
            securityDefinition.ClaimDefinitions.Add(new MetadataSecurityClaimDefinition(ClaimCheckerFactory.CreateCustomChecker(handler)));

            _metadataRouteDefinition = _metadataRouteDefinition with
            {
                SecurityDefinition = securityDefinition
            };
        }

        void HasNoValue()
        {
            var securityDefinition = new MetadataSecurityDefinition(new(),
                                                                    new(),
                                                                    new());

            HasValue(securityDefinition);
        }

        _metadataRouteDefinition.SecurityDefinition.DoSwitch(HasValue, HasNoValue);

        return this;
    }

    /// <summary>
    /// Configure role authorizations
    /// </summary>
    public MetadataRouteDefinitionBuilder RequireRoleAuthorization(params string[] roles)
    {
        void HasValue(MetadataSecurityDefinition securityDefinition)
        {
            securityDefinition.RoleDefinitions.AddRange(roles.Select(x => new MetadataSecurityRoleDefinition(x)));

            _metadataRouteDefinition = _metadataRouteDefinition with
            {
                SecurityDefinition = securityDefinition
            };
        }

        void HasNoValue()
        {
            var securityDefinition = new MetadataSecurityDefinition(new(),
                                                                    new(),
                                                                    new());

            HasValue(securityDefinition);
        }

        _metadataRouteDefinition.SecurityDefinition.DoSwitch(HasValue, HasNoValue);

        return this;
    }
}