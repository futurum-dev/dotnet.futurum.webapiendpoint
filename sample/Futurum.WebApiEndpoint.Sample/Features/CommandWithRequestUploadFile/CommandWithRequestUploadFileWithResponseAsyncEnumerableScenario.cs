using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFile;

public static class CommandWithRequestUploadFileWithResponseAsyncEnumerableScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFile<ApiEndpoint>.WithResponseAsyncEnumerable<FeatureDto, Feature>.WithMapper<FeatureDataMapper>
    {
        protected override Task<Result<ResponseAsyncEnumerable<Feature>>> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken)
        {
            return new ResponseAsyncEnumerable<Feature>(AsyncEnumerable()).ToResultOkAsync();

            async IAsyncEnumerable<Feature> AsyncEnumerable()
            {
                await Task.Yield();

                foreach (var i in Enumerable.Range(0, 10))
                {
                    yield return new Feature($"Name - {i} - {command.File.FileName}");
                }
            }
        }
    }
}