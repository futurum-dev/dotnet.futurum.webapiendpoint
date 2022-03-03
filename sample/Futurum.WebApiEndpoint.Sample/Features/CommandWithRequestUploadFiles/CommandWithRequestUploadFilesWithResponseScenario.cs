using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFiles;

public static class CommandWithRequestUploadFilesWithResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFiles.Response<FeatureDto, Feature>.Mapper<FeatureMapper>
    {
        public override Task<Result<Feature>> ExecuteAsync(RequestUploadFiles request, CancellationToken cancellationToken) =>
            Enumerable.Range(0, 10)
                      .Select(i => new Feature($"Name - {i} - {request.Files.Single().FileName}"))
                      .First()
                      .ToResultOkAsync();
    }
}