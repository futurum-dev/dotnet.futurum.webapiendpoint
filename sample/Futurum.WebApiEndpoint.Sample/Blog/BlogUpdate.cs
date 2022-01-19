using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Blog;

public static class BlogUpdate
{
    public record CommandDto(long Id, string Url);
    
    public record Command(Blog Blog);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponse<BlogDto, Blog>
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
        public Result<Command> Map(HttpContext httpContext, CommandDto dto) =>
            new Command(new Blog(dto.Id.ToId(), dto.Url)).ToResultOk();
    }
}