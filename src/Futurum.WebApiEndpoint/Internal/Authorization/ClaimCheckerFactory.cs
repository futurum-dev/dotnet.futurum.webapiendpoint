using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Authorization;

namespace Futurum.WebApiEndpoint.Internal.Authorization;

public static class ClaimCheckerFactory
{
    public static IClaimChecker CreateEqualityChecker(ClaimRecord claimRecord) =>
        new EqualityClaimChecker(new[] { claimRecord });

    public static IClaimChecker CreateEqualityChecker(IEnumerable<ClaimRecord> claimRecords) =>
        new EqualityClaimChecker(claimRecords);

    public static IClaimChecker CreateCustomChecker(Func<AuthorizationHandlerContext, bool> handler) =>
        new CustomClaimChecker(handler);
}