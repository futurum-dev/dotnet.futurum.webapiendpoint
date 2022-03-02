using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Blog;

public static class BlogUpdate
{
    public record CommandDto(long Id, string Url);
    
    public record Command(Blog Blog);

    public class ApiEndpoint : CommandWebApiEndpoint.Request<CommandDto, Command>.Response<BlogDto, Blog>.Mapper<Mapper, BlogMapper>
    {
        private readonly IBlogStorageBroker _storageBroker;

        public ApiEndpoint(IBlogStorageBroker storageBroker)
        {
            _storageBroker = storageBroker;
        }

        protected override Task<Result<Blog>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            _storageBroker.UpdateAsync(command.Blog);
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
    {
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CommandDto dto, CancellationToken cancellationToken) =>
            new Command(new Blog(dto.Id.ToId(), dto.Url)).ToResultOkAsync();
    }
}