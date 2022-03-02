using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFiles;

public static class CommandWithRequestUploadFilesWithoutResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFiles<ApiEndpoint>.NoResponse
    {
        protected override Task<Result> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken) =>
            Result.OkAsync();
    }
}