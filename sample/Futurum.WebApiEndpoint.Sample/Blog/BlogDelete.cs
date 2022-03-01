using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Blog;

public static class BlogDelete
{
    public record Command(Id Id);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<Command>.WithoutResponse.WithMapper<Mapper>
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
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
            httpContext.GetRequestPathParameterAsLong("Id")
                       .Map(id => new Command(id.ToId()))
                       .ToResultAsync();
    }
}