using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Authorization;

namespace Futurum.WebApiEndpoint.Internal.Authorization;

internal class EqualityClaimChecker : IClaimChecker
{
    public EqualityClaimChecker(IEnumerable<ClaimRecord> claimRecords)
    {
        ClaimRecords = claimRecords.ToArray();
    }

    public IEnumerable<ClaimRecord> ClaimRecords { get; }

    public bool Execute(AuthorizationHandlerContext authorizationHandlerContext) =>
        !ClaimRecords.Except(authorizationHandlerContext.User.Claims.Select(x => new ClaimRecord(x.Type, x.Value))).Any();
}

public record ClaimRecord(string Type, string Name);