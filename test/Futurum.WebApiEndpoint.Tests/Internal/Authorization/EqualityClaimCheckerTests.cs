using System.Security.Claims;

using FluentAssertions;

using Futurum.WebApiEndpoint.Internal.Authorization;

using Microsoft.AspNetCore.Authorization;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.Internal.Authorization;

public class EqualityClaimCheckerTests
{
    [Fact]
    public void exact_match()
    {
        var claimRecords = Enumerable.Range(0, 10)
                                     .Select(_ => new ClaimRecord(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()))
                                     .ToList();

        var equalityClaimChecker = new EqualityClaimChecker(claimRecords);

        var authorizationHandlerContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement>(),
                                                                          new ClaimsPrincipal(new ClaimsIdentity(claimRecords.Select(x => new Claim(x.Type, x.Name)).ToList())),
                                                                          null);

        var result = equalityClaimChecker.Execute(authorizationHandlerContext);

        result.Should().BeTrue();
    }

    [Fact]
    public void user_has_all_and_more()
    {
        var claimRecords = Enumerable.Range(0, 10)
                                     .Select(_ => new ClaimRecord(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()))
                                     .ToList();

        var equalityClaimChecker = new EqualityClaimChecker(claimRecords.Take(1));

        var authorizationHandlerContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement>(),
                                                                          new ClaimsPrincipal(new ClaimsIdentity(claimRecords.Select(x => new Claim(x.Type, x.Name)).ToList())),
                                                                          null);

        var result = equalityClaimChecker.Execute(authorizationHandlerContext);

        result.Should().BeTrue();
    }

    [Fact]
    public void user_has_less()
    {
        var claimRecords = Enumerable.Range(0, 10)
                                     .Select(_ => new ClaimRecord(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()))
                                     .ToList();

        var equalityClaimChecker = new EqualityClaimChecker(claimRecords);

        var authorizationHandlerContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement>(),
                                                                          new ClaimsPrincipal(new ClaimsIdentity(claimRecords.Take(1).Select(x => new Claim(x.Type, x.Name)).ToList())),
                                                                          null);

        var result = equalityClaimChecker.Execute(authorizationHandlerContext);

        result.Should().BeFalse();
    }

    [Fact]
    public void user_has_none()
    {
        var claimRecords = Enumerable.Range(0, 10)
                                     .Select(_ => new ClaimRecord(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()))
                                     .ToList();

        var equalityClaimChecker = new EqualityClaimChecker(claimRecords);

        var authorizationHandlerContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement>(),
                                                                          new ClaimsPrincipal(new ClaimsIdentity(claimRecords.Take(0).Select(x => new Claim(x.Type, x.Name)).ToList())),
                                                                          null);

        var result = equalityClaimChecker.Execute(authorizationHandlerContext);

        result.Should().BeFalse();
    }
}