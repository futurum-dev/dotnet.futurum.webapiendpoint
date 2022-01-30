using FluentAssertions;

using Futurum.Core.Linq;

using Microsoft.AspNetCore.Http;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class RequestUploadFilesTests
{
    [Fact]
    public async Task ToNonApiEndpoint()
    {
        await using var fileStream = File.OpenRead("./Data/hello-world.txt");
        
        var formFile = new FormFile(fileStream, 0, fileStream.Length, "hello-world.txt", "hello-world.txt");

        var generic = new RequestUploadFiles<object>(EnumerableExtensions.Return(formFile));

        var nonGeneric = generic.ToNonApiEndpoint();

        generic.Files.Should().BeEquivalentTo(nonGeneric.Files);
    }
}