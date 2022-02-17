using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class CommandWithRequestUploadFilesWithResponseDataCollectionScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFiles<ApiEndpoint>.WithResponseDataCollection<FeatureDto, Feature>.WithMapper<FeatureDataMapper>
    {
        protected override Task<Result<ResponseDataCollection<Feature>>> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken) =>
            new ResponseDataCollection<Feature>(Enumerable.Range(0, 10).Select(i => new Feature($"Name - {i} - {command.Files.First().FileName}"))).ToResultOkAsync();
    }
}