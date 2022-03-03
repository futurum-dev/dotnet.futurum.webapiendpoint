using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestPlainText;

public static class CommandWithRequestPlainTextWithResponseEmptyJsonScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestPlainText.ResponseEmptyJson
    {
        protected override Task<Result<ResponseEmptyJson>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken) =>
            new ResponseEmptyJson().ToResultOkAsync();
    }
}