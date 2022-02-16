using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Blog;

public static class BlogGetById
{
    public record QueryDto
    {
        [MapFromPath("Id")] public long Id { get; set; }
    }

    public record Query(Id Id);

    public class ApiEndpoint : QueryWebApiEndpoint.WithRequest<QueryDto, Query>.WithResponse<BlogDto, Blog>.WithMapper<Mapper, BlogMapper>
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
        public Result<Query> Map(HttpContext httpContext, QueryDto dto) =>
            new Query(dto.Id.ToId()).ToResultOk();
    }
}