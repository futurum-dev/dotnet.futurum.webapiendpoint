using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFile;

public static class CommandWithRequestUploadFileWithResponseAsyncEnumerableScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFile<ApiEndpoint>.ResponseAsyncEnumerable<FeatureDto, Feature>.Mapper<FeatureDataMapper>
    {
        protected override Task<Result<ResponseAsyncEnumerable<Feature>>> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken) =>
            new ResponseAsyncEnumerable<Feature>(AsyncEnumerable.Range(0, 10).Select(i => new Feature($"Name - {i} - {command.File.FileName}"))).ToResultOkAsync();
    }
}