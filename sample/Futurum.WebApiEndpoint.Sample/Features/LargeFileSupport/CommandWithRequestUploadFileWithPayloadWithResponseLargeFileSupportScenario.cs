using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.LargeFileSupport;

public static class CommandWithRequestUploadFileWithPayloadWithResponseLargeFileSupportScenario
{
    public record RequestDto
    {
        [MapFromMultipartFile(0)] public IFormFile File { get; set; }
    }

    public record Request(IFormFile File);
    
    public class ApiEndpoint : CommandWebApiEndpoint.RequestMapFromMultipart<RequestDto, Request>.Response<FeatureDto, Feature>.Mapper<Mapper, FeatureMapper>
    {
        public override Task<Result<Feature>> ExecuteAsync(Request request, CancellationToken cancellationToken) =>
            Enumerable.Range(0, 10)
                      .Select(i => new Feature($"Name - {i} - {request.File.FileName}"))
                      .First()
                      .ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>
    {
        public Task<Result<Request>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, RequestDto dto, CancellationToken cancellationToken) =>
            new Request(dto.File).ToResultOkAsync();
    }
}