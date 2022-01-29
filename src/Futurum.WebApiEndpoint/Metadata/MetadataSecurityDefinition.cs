using Microsoft.AspNetCore.Authorization;

namespace Futurum.WebApiEndpoint.Metadata;

public record MetadataSecurityDefinition(List<MetadataSecurityPermissionDefinition> PermissionDefinitions,
                                         List<MetadataSecurityRoleDefinition> RoleDefinitions,
                                         List<MetadataSecurityClaimDefinition> ClaimDefinitions);

public record MetadataSecurityPermissionDefinition(string Name);

public record MetadataSecurityRoleDefinition(string Name);

public record MetadataSecurityClaimDefinition(IClaimChecker ClaimChecker);

public interface IClaimChecker
{
    bool Execute(AuthorizationHandlerContext authorizationHandlerContext);
}