using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Blog;

public static class BlogGetById
{
    public record QueryDto
    {
        [MapFromPath("Id")] public long Id { get; set; }
    }

    public record Query(Id Id);

    public class ApiEndpoint : QueryWebApiEndpoint.Request<QueryDto, Query>.Response<BlogDto, Blog>.Mapper<Mapper, BlogMapper>
    {
        private readonly IBlogStorageBroker _storageBroker;

        public ApiEndpoint(IBlogStorageBroker storageBroker)
        {
            _storageBroker = storageBroker;
        }

        protected override Task<Result<Blog>> ExecuteAsync(Query query, CancellationToken cancellationToken) =>
            _storageBroker.GetByIdAsync(query.Id);
    }

    public class Mapper : IWebApiEndpointRequestMapper<QueryDto, Query>
    {
        public Task<Result<Query>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, QueryDto dto, CancellationToken cancellationToken) =>
            new Query(dto.Id.ToId()).ToResultOkAsync();
    }
}