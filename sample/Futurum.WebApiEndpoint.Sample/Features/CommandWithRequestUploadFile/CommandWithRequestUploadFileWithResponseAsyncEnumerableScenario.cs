using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFile;

public static class CommandWithRequestUploadFileWithResponseAsyncEnumerableScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFile.ResponseAsyncEnumerable<FeatureDto, Feature>.Mapper<FeatureDataMapper>
    {
        public override Task<Result<ResponseAsyncEnumerable<Feature>>> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken) =>
            new ResponseAsyncEnumerable<Feature>(AsyncEnumerable.Range(0, 10).Select(i => new Feature($"Name - {i} - {command.File.FileName}"))).ToResultOkAsync();
    }
}