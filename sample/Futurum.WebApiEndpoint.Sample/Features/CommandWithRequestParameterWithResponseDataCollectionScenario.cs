using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class CommandWithRequestParameterWithResponseDataCollectionScenario
{
    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<Command>.WithResponseDataCollection<ApiEndpoint, FeatureDto, Feature>.WithMapper<Mapper, FeatureDataMapper>
    {
        protected override Task<Result<ResponseDataCollection<Feature>>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            new ResponseDataCollection<Feature>(Enumerable.Range(0, 10).Select(i => new Feature($"Name - {i} - {command.Id}"))).ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<Command>
    {
        public Result<Command> Map(HttpContext httpContext) =>
            httpContext.GetRequestPathParameterAsString("Id")
                       .Map(id => new Command(id));
    }
}