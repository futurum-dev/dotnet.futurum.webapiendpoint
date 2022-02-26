using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestManualParameter;

public static class CommandWithRequestManualParameterWithResponseFileStreamScenario
{
    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<Command>.WithResponseFileStream<ApiEndpoint>.WithMapper<Mapper>
    {
        protected override Task<Result<ResponseFileStream>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/hello-world.txt")).ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<Command>
    {
        public Result<Command> Map(HttpContext httpContext) =>
            httpContext.GetRequestPathParameterAsString("Id")
                       .Map(id => new Command(id));
    }
}