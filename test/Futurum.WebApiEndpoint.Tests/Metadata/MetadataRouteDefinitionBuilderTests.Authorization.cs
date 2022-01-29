using FluentAssertions;

using Futurum.Core.Option;
using Futurum.Test.Option;
using Futurum.WebApiEndpoint.Internal.Authorization;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.Metadata;

public class MetadataRouteDefinitionBuilderAuthorizationTests
{
    public class ApiEndpoint
    {
    }

    public class AllowAnonymousAuthorization
    {
        [Fact]
        public void when_specified()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0,
                                                                      0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .AllowAnonymousAuthorization();

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.SecurityDefinition.ShouldBeHasNoValue();
        }

        [Fact]
        public void when_not_specified()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.SecurityDefinition.ShouldBeHasNoValue();
        }
    }

    public class RequireAuthorization
    {
        [Fact]
        public void when_specified()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0,
                                                                      0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .RequireAuthorization();

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.SecurityDefinition.ShouldBeHasValue();
            resultMetadataRouteDefinition.SecurityDefinition.ShouldBeHasValueWithValueEquivalentTo(new(new(),
                                                                                                       new(),
                                                                                                       new()));
        }

        [Fact]
        public void when_not_specified()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.SecurityDefinition.ShouldBeHasNoValue();
        }
    }

    public class RequirePermissionAuthorization
    {
        [Fact]
        public void when_specified()
        {
            var permissions = Enumerable.Range(0, 10)
                                        .Select(_ => Guid.NewGuid().ToString())
                                        .ToList();

            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0,
                                                                      0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .RequirePermissionAuthorization(permissions.ToArray());

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.SecurityDefinition.ShouldBeHasValue();
            resultMetadataRouteDefinition.SecurityDefinition.ShouldBeHasValueWithValueEquivalentTo(new(
                                                                                                       permissions.Select(x => new MetadataSecurityPermissionDefinition(x)).ToList(),
                                                                                                       new(),
                                                                                                       new()));
        }

        [Fact]
        public void when_not_specified()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.SecurityDefinition.ShouldBeHasNoValue();
        }
    }

    public class RequireClaimAuthorization
    {
        [Fact]
        public void when_single_type_and_name_specified()
        {
            var claimType = Guid.NewGuid().ToString();
            var claim = Guid.NewGuid().ToString();
        
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 0,
                                                                      0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);
        
            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .RequireClaimAuthorization(claimType, claim);
        
            var metadataRouteDefinitions = builder.Build();
        
            metadataRouteDefinitions.Count().Should().Be(1);
        
            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.SecurityDefinition.ShouldBeHasValue();
            resultMetadataRouteDefinition.SecurityDefinition.ShouldBeHasValueWithValueAssertion(metadataSecurityDefinition =>
            {
                metadataSecurityDefinition.PermissionDefinitions.Should().BeEmpty();
                metadataSecurityDefinition.RoleDefinitions.Should().BeEmpty();
                metadataSecurityDefinition.ClaimDefinitions.Count.Should().Be(1);

                var claimDefinition = metadataSecurityDefinition.ClaimDefinitions.Single();

                claimDefinition.ClaimChecker.Should().BeOfType<EqualityClaimChecker>();

                var equalityClaimChecker = claimDefinition.ClaimChecker as EqualityClaimChecker;
                equalityClaimChecker.ClaimRecords.Should().BeEquivalentTo(new List<ClaimRecord>{new ClaimRecord(claimType, claim)});
            });
        }
        
        [Fact]
        public void when_multiple_type_and_name_specified()
        {
            var claimRecords = Enumerable.Range(0, 10)
                                         .Select(_ => new ClaimRecord(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()))
                                         .ToList();
        
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 0,
                                                                      0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);
        
            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .RequireClaimAuthorization(claimRecords.Select(claimRecord => (claimRecord.Type, claimRecord.Name)).ToArray());
        
            var metadataRouteDefinitions = builder.Build();
        
            metadataRouteDefinitions.Count().Should().Be(1);
        
            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.SecurityDefinition.ShouldBeHasValue();
            resultMetadataRouteDefinition.SecurityDefinition.ShouldBeHasValueWithValueAssertion(metadataSecurityDefinition =>
            {
                metadataSecurityDefinition.PermissionDefinitions.Should().BeEmpty();
                metadataSecurityDefinition.RoleDefinitions.Should().BeEmpty();
                metadataSecurityDefinition.ClaimDefinitions.Count.Should().Be(1);

                var claimDefinition = metadataSecurityDefinition.ClaimDefinitions.Single();

                claimDefinition.ClaimChecker.Should().BeOfType<EqualityClaimChecker>();

                var equalityClaimChecker = claimDefinition.ClaimChecker as EqualityClaimChecker;
                equalityClaimChecker.ClaimRecords.Should().BeEquivalentTo(claimRecords);
            });
        }

        [Fact]
        public void when_handler_specified()
        {
            var handler = (AuthorizationHandlerContext authorizationHandlerContext) => true;

            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0,
                                                                      0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .RequireClaimAuthorization(handler);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.SecurityDefinition.ShouldBeHasValue();
            resultMetadataRouteDefinition.SecurityDefinition.ShouldBeHasValueWithValueAssertion(metadataSecurityDefinition =>
            {
                metadataSecurityDefinition.PermissionDefinitions.Should().BeEmpty();
                metadataSecurityDefinition.RoleDefinitions.Should().BeEmpty();
                metadataSecurityDefinition.ClaimDefinitions.Count.Should().Be(1);

                var claimDefinition = metadataSecurityDefinition.ClaimDefinitions.Single();

                claimDefinition.ClaimChecker.Should().BeOfType<CustomClaimChecker>();

                var customClaimChecker = claimDefinition.ClaimChecker as CustomClaimChecker;
                customClaimChecker.Handler.Should().Be(handler);
            });
        }

        [Fact]
        public void when_not_specified()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.SecurityDefinition.ShouldBeHasNoValue();
        }
    }

    public class RequireRoleAuthorization
    {
        [Fact]
        public void when_specified()
        {
            var roles = Enumerable.Range(0, 10)
                                        .Select(_ => Guid.NewGuid().ToString())
                                        .ToList();

            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0,
                                                                      0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .RequireRoleAuthorization(roles.ToArray());

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.SecurityDefinition.ShouldBeHasValue();
            resultMetadataRouteDefinition.SecurityDefinition.ShouldBeHasValueWithValueEquivalentTo(new(
                                                                                                       new(),
                                                                                                       roles.Select(x => new MetadataSecurityRoleDefinition(x)).ToList(),
                                                                                                       new()));
        }

        [Fact]
        public void when_not_specified()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.SecurityDefinition.ShouldBeHasNoValue();
        }
    }
}