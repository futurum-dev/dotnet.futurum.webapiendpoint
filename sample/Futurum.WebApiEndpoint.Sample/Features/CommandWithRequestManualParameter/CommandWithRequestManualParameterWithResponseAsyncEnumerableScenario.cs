using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestManualParameter;

public static class CommandWithRequestManualParameterWithResponseAsyncEnumerableScenario
{
    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<Command>.WithResponseAsyncEnumerable<ApiEndpoint, FeatureDto, Feature>.WithMapper<Mapper, FeatureDataMapper>
    {
        protected override Task<Result<ResponseAsyncEnumerable<Feature>>> ExecuteAsync(Command command, CancellationToken cancellationToken)
        {
            return new ResponseAsyncEnumerable<Feature>(AsyncEnumerable()).ToResultOkAsync();

            async IAsyncEnumerable<Feature> AsyncEnumerable()
            {
                await Task.Yield();

                foreach (var i in Enumerable.Range(0, 10))
                {
                    yield return new Feature($"Name - {i} - {command.Id}");
                }
            }
        }
    }

    public class Mapper : IWebApiEndpointRequestMapper<Command>
    {
        public Result<Command> Map(HttpContext httpContext) =>
            httpContext.GetRequestPathParameterAsString("Id")
                       .Map(id => new Command(id));
    }
}