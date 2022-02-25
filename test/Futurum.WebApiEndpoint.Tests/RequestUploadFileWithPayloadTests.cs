using FluentAssertions;

using Futurum.Core.Linq;

using Microsoft.AspNetCore.Http;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class RequestUploadFileWithPayloadTests
{
    [Fact]
    public async Task ToNonApiEndpoint()
    {
        await using var fileStream = File.OpenRead("./Data/hello-world.txt");
        
        var formFile = new FormFile(fileStream, 0, fileStream.Length, "hello-world.txt", "hello-world.txt");

        var generic = new RequestUploadFileWithPayload<object, string>(formFile, Guid.NewGuid().ToString());

        var nonGeneric = generic.ToNonApiEndpoint();

        nonGeneric.File.Should().BeEquivalentTo(generic.File);
        nonGeneric.Payload.Should().BeEquivalentTo(generic.Payload);
    }
}