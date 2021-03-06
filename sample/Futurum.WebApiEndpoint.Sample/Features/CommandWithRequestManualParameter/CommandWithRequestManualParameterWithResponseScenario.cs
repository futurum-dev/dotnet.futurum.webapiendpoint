using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestManualParameter;

public static class CommandWithRequestManualParameterWithResponseScenario
{
    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.Request<Command>.Response<FeatureDto, Feature>.Mapper<Mapper, FeatureMapper>
    {
        public override Task<Result<Feature>> ExecuteAsync(Command request, CancellationToken cancellationToken) =>
            new Feature($"Name - {request.Id}").ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<Command>
    {
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
            httpContext.GetRequestPathParameterAsString("Id")
                       .Map(id => new Command(id))
                       .ToResultAsync();
    }
}