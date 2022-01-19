using System.Collections.Immutable;

using Microsoft.AspNetCore.Authorization;

namespace Futurum.WebApiEndpoint.Metadata;

public record MetadataSecurityDefinition(ImmutableList<MetadataSecurityPermissionDefinition> PermissionDefinitions,
                                         ImmutableList<MetadataSecurityRoleDefinition> RoleDefinitions,
                                         ImmutableList<MetadataSecurityClaimDefinition> ClaimDefinitions);

public record MetadataSecurityPermissionDefinition(string Name);
public record MetadataSecurityRoleDefinition(string Name);
public record MetadataSecurityClaimDefinition(Func<AuthorizationHandlerContext, bool> Handler);

public static class MetadataSecurityDefinitionHandlers
{
    public static Func<AuthorizationHandlerContext, bool> ClaimEqualityCheck(IEnumerable<ClaimRecord> claimRecords)
    {
        var claimsRequired = claimRecords.ToArray();

        return authorizationHandlerContext => !claimsRequired.Except(authorizationHandlerContext.User.Claims.Select(x => new ClaimRecord(x.Type, x.Value))).Any();
    }

    public record ClaimRecord(string Type, string Name);
}