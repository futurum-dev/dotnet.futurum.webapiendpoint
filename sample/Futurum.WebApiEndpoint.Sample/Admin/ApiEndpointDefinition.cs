using Futurum.ApiEndpoint;

namespace Futurum.WebApiEndpoint.Sample.Admin;

public class ApiEndpointDefinition : IApiEndpointDefinition
{
    public void Configure(ApiEndpointDefinitionBuilder definitionBuilder)
    {
        definitionBuilder.Web()
                         .Query<GetUsers.ApiEndpoint>(builder => builder.Route("get-users-requires-authorization").RequireAuthorization()
                                                                        .Version(WebApiEndpointVersions.V1_0, WebApiEndpointVersions.V2_0)
                                                                        .Summary("Get users").Description("Get users"))
                         .Query<GetUsers.ApiEndpoint>(builder => builder.Route("get-users-requires-permission").RequirePermissionAuthorization(Authorization.Permission.Admin)
                                                                        .Version(WebApiEndpointVersions.V1_0, WebApiEndpointVersions.V2_0)
                                                                        .Summary("Get users").Description("Get users"))
                         .Query<GetUsers.ApiEndpoint>(builder => builder.Route("get-users-requires-claim").RequireClaimAuthorization(Authorization.ClaimType.Admin, Authorization.Claim.Admin)
                                                                        .Version(WebApiEndpointVersions.V1_0, WebApiEndpointVersions.V2_0)
                                                                        .Summary("Get users").Description("Get users"))
                         .Query<GetUsers.ApiEndpoint>(builder => builder.Route("get-users-requires-role").RequireRoleAuthorization(Authorization.Role.Admin)
                                                                        .Version(WebApiEndpointVersions.V1_0, WebApiEndpointVersions.V2_0)
                                                                        .Summary("Get users").Description("Get users"));
    }
}