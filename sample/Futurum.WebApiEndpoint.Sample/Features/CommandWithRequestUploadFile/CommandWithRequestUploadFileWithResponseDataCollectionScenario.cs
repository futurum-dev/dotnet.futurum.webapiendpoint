using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFile;

public static class CommandWithRequestUploadFileWithResponseDataCollectionScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFile.ResponseDataCollection<FeatureDto, Feature>.Mapper<FeatureDataMapper>
    {
        protected override Task<Result<ResponseDataCollection<Feature>>> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken) =>
            new ResponseDataCollection<Feature>(Enumerable.Range(0, 10).Select(i => new Feature($"Name - {i} - {command.File.FileName}"))).ToResultOkAsync();
    }
}