using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequest;

public static class CommandWithRequestWithResponseAsyncEnumerableScenario
{
    public record CommandDto(string Id);

    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.Request<CommandDto, Command>.ResponseAsyncEnumerable<FeatureDto, Feature>.Mapper<Mapper, FeatureDataMapper>
    {
        public override Task<Result<ResponseAsyncEnumerable<Feature>>> ExecuteAsync(Command request, CancellationToken cancellationToken) =>
            new ResponseAsyncEnumerable<Feature>(AsyncEnumerable.Range(0, 10).Select(i => new Feature($"Name - {i} - {request.Id}"))).ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
    {
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CommandDto dto, CancellationToken cancellationToken) =>
            new Command(dto.Id).ToResultOkAsync();
    }
}