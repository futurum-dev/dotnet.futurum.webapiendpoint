using Futurum.Core.Result;
using Futurum.Test.Result;

using Microsoft.AspNetCore.Http;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class RequestUploadFileWithPayloadMapperTests
{
    [Fact]
    public async Task Map()
    {
        await using var fileStream = File.OpenRead("./Data/hello-world.txt");

        var formFile = new FormFile(fileStream, 0, fileStream.Length, "hello-world.txt", "hello-world.txt");

        var payload = Guid.NewGuid().ToString();
        
        var nonGeneric = new RequestUploadFileWithPayloadDto<string>
        {
            File = formFile,
            Payload = payload
        };

        var result = new RequestUploadFileWithPayloadMapper<object, string, string, PayloadMapper>(new PayloadMapper()).Map(new DefaultHttpContext(), nonGeneric);

        result.ShouldBeSuccessWithValueEquivalentTo(new RequestUploadFileWithPayload<object, string>(formFile, payload));
    }

    private class PayloadMapper : IWebApiEndpointRequestPayloadMapper<string, string>
    {
        public Result<string> Map(string dto) =>
            dto.ToResultOk();
    }
}