using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFile;

public static class CommandWithRequestUploadFileWithResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFile.Response<FeatureDto, Feature>.Mapper<FeatureMapper>
    {
        protected override Task<Result<Feature>> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken) =>
            Enumerable.Range(0, 10)
                      .Select(i => new Feature($"Name - {i} - {command.File.FileName}"))
                      .First()
                      .ToResultOkAsync();
    }
}