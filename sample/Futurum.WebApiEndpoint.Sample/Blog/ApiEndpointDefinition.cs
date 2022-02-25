using Futurum.ApiEndpoint;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Blog;

public class ApiEndpointDefinition : IApiEndpointDefinition
{
    public void Configure(ApiEndpointDefinitionBuilder definitionBuilder)
    {
        definitionBuilder.Web()
                         .Command<BlogCreate.ApiEndpoint>(builder => builder.Post("blog").Version(WebApiEndpointVersions.V1_0).Summary("Create Blog").Description("Create Blog"))
                         .Command<BlogDelete.ApiEndpoint>(builder => builder.Delete("blog/{Id}", ("Id", MetadataRouteParameterDefinitionType.Path, typeof(long)))
                                                                            .Version(WebApiEndpointVersions.V1_0).Summary("Delete Blog").Description("Delete Blog"))
                         .Query<BlogGet.ApiEndpoint>(builder => builder.Route("blog").Version(WebApiEndpointVersions.V1_0).Summary("Get Blogs").Description("Get Blogs"))
                         .Query<BlogGetById.ApiEndpoint>(builder => builder.Route("blog/{Id}")
                                                                           .Version(WebApiEndpointVersions.V1_0).Summary("Get Blogs By Id").Description("Get Blogs By Id"))
                         .Command<BlogUpdate.ApiEndpoint>(builder => builder.Put("blog").Version(WebApiEndpointVersions.V1_0).Summary("Update Blog").Description("Update Blog"))
                         .Query<BlogGetAsyncEnumerable.ApiEndpoint>(builder => builder.Route("blog-async").Version(WebApiEndpointVersions.V1_0).Summary("Get Blogs").Description("Get Blogs"));
    }
}