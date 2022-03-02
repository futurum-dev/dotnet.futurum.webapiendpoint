using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestManualParameter;

public static class CommandWithRequestManualParameterWithResponseAsyncEnumerableScenario
{
    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.Request<Command>.ResponseAsyncEnumerable<ApiEndpoint, FeatureDto, Feature>.Mapper<Mapper, FeatureDataMapper>
    {
        protected override Task<Result<ResponseAsyncEnumerable<Feature>>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            new ResponseAsyncEnumerable<Feature>(AsyncEnumerable.Range(0, 10).Select(i => new Feature($"Name - {i} - {command.Id}"))).ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<Command>
    {
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
            httpContext.GetRequestPathParameterAsString("Id")
                       .Map(id => new Command(id))
                       .ToResultAsync();
    }
}