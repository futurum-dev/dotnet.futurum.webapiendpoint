using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Blog;

public class BlogDataMapper : IWebApiEndpointResponseDataMapper<Blog, BlogDto>
{
    public BlogDto Map(Blog data) => 
        new(data.Id.GetValueOrDefault(x => x.Value, 0), data.Url);
}

public class BlogMapper : IWebApiEndpointResponseMapper<Blog, BlogDto>
{
    public BlogDto Map(HttpContext httpContext, Blog domain) => 
        new(domain.Id.GetValueOrDefault(x => x.Value, 0), domain.Url);

    public static Result<Blog> MapToDomain(BlogDto dto) => 
        new Blog(dto.Id.ToId(), dto.Url).ToResultOk();
}