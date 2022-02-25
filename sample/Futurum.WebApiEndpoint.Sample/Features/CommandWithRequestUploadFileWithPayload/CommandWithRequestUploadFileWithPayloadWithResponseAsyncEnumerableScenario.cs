using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFileWithPayload;

public static class CommandWithRequestUploadFileWithPayloadWithResponseAsyncEnumerableScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFileWithPayload<ApiEndpoint, PayloadDto, Payload>.WithResponseAsyncEnumerable<FeatureDto, Feature>.WithMapper<PayloadMapper, FeatureDataMapper>
    {
        protected override Task<Result<ResponseAsyncEnumerable<Feature>>> ExecuteAsync(RequestUploadFileWithPayload<Payload> command, CancellationToken cancellationToken)
        {
            return new ResponseAsyncEnumerable<Feature>(AsyncEnumerable()).ToResultOkAsync();

            async IAsyncEnumerable<Feature> AsyncEnumerable()
            {
                await Task.Yield();

                foreach (var i in Enumerable.Range(0, 10))
                {
                    yield return new Feature($"Name - {i} - {command.File.FileName} - {command.Payload.Id}");
                }
            }
        }
    }
}