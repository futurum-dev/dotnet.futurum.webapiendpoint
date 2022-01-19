using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Blog;

public class BlogDataMapper : IWebApiEndpointDataMapper<Blog, BlogDto>
{
    public BlogDto Map(Blog data) => 
        new(data.Id.GetValueOrDefault(x => x.Value, 0), data.Url);
}

public class BlogMapper : IWebApiEndpointResponseMapper<Blog, BlogDto>
{
    public Result<BlogDto> Map(Blog domain) =>
        MapToDto(domain);
    
    public static Result<BlogDto> MapToDto(Blog domain) => 
        new BlogDto(domain.Id.GetValueOrDefault(x => x.Value, 0), domain.Url).ToResultOk();

    public static Result<Blog> MapToDomain(BlogDto dto) => 
        new Blog(dto.Id.ToId(), dto.Url).ToResultOk();
}