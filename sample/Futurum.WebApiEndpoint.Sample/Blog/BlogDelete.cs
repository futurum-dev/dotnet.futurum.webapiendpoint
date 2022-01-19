using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;

namespace Futurum.WebApiEndpoint.Sample.Blog;

public static class BlogDelete
{
    public record Command(Id Id);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<Command>.WithoutResponse
    {
        private readonly IBlogStorageBroker _storageBroker;

        public ApiEndpoint(IBlogStorageBroker storageBroker)
        {
            _storageBroker = storageBroker;
        }

        protected override Task<Result> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            _storageBroker.DeleteAsync(command.Id);
    }

    public class Mapper : IWebApiEndpointRequestMapper<Command>
    {
        public Result<Command> Map(HttpContext httpContext) =>
            httpContext.GetRequestPathParameterAsLong("Id")
                       .Map(id => new Command(id.ToId()));
    }
}