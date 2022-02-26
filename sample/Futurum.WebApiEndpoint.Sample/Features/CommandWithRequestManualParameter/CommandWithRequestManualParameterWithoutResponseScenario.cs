using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestManualParameter;

public static class CommandWithRequestManualParameterWithoutResponseScenario
{
    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<Command>.WithoutResponse.WithMapper<Mapper>
    {
        protected override Task<Result> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            Result.OkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<Command>
    {
        public Result<Command> Map(HttpContext httpContext) =>
            httpContext.GetRequestPathParameterAsString("Id")
                       .Map(id => new Command(id));
    }
}