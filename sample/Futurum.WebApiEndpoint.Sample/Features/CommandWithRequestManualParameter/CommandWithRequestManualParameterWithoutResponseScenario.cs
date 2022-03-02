using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestManualParameter;

public static class CommandWithRequestManualParameterWithoutResponseScenario
{
    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.Request<Command>.NoResponse.Mapper<Mapper>
    {
        protected override Task<Result> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            Result.OkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<Command>
    {
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
            httpContext.GetRequestPathParameterAsString("Id")
                       .Map(id => new Command(id))
                       .ToResultAsync();
    }
}