using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestPlainText;

public static class CommandWithRequestPlainTextWithoutResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestPlainText<ApiEndpoint>.WithoutResponse
    {
        protected override Task<Result> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken) =>
            Result.OkAsync();
    }
}