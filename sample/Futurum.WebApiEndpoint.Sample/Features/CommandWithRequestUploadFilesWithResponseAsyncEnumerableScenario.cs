using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class CommandWithRequestUploadFilesWithResponseAsyncEnumerableScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFiles<ApiEndpoint>.WithResponseAsyncEnumerable<FeatureDto, Feature>.WithMapper<FeatureDataMapper>
    {
        protected override Task<Result<ResponseAsyncEnumerable<Feature>>> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken)
        {
            return new ResponseAsyncEnumerable<Feature>(AsyncEnumerable()).ToResultOkAsync();

            async IAsyncEnumerable<Feature> AsyncEnumerable()
            {
                await Task.Yield();

                foreach (var i in Enumerable.Range(0, 10))
                {
                    yield return new Feature($"Name - {i} - {command.Files.First().FileName}");
                }
            }
        }
    }
}