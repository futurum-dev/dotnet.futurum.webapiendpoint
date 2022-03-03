using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFiles;

public static class CommandWithRequestUploadFilesWithResponseDataCollectionScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFiles.ResponseDataCollection<FeatureDto, Feature>.Mapper<FeatureDataMapper>
    {
        public override Task<Result<ResponseDataCollection<Feature>>> ExecuteAsync(RequestUploadFiles request, CancellationToken cancellationToken) =>
            new ResponseDataCollection<Feature>(Enumerable.Range(0, 10).Select(i => new Feature($"Name - {i} - {request.Files.First().FileName}"))).ToResultOkAsync();
    }
}