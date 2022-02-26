using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestManualParameter;

public static class CommandWithRequestManualParameterWithResponseEmptyJsonScenario
{
    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<Command>.WithResponseEmptyJson<ApiEndpoint>.WithMapper<Mapper>
    {
        protected override Task<Result<ResponseEmptyJson>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            new ResponseEmptyJson().ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<Command>
    {
        public Result<Command> Map(HttpContext httpContext) =>
            httpContext.GetRequestPathParameterAsString("Id")
                       .Map(id => new Command(id));
    }
}