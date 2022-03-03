using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestManualParameter;

public static class CommandWithRequestManualParameterWithResponseBytesScenario
{
    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.Request<Command>.ResponseBytes.Mapper<Mapper>
    {
        protected override Task<Result<ResponseBytes>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            new ResponseBytes(File.ReadAllBytes("./Data/hello-world.txt"), $"hello-world-bytes-{command.Id}").ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<Command>
    {
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
            httpContext.GetRequestPathParameterAsString("Id")
                       .Map(id => new Command(id))
                       .ToResultAsync();
    }
}