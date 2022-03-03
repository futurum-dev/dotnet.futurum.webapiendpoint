using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestPlainText;

public static class CommandWithRequestPlainTextWithoutResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestPlainText.NoResponse
    {
        public override Task<Result<ResponseEmpty>> ExecuteAsync(RequestPlainText request, CancellationToken cancellationToken) =>
            ResponseEmpty.DefaultResultAsync;
    }
}