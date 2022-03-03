using FluentAssertions;

using Futurum.Test.Result;

using Microsoft.AspNetCore.Http;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class RequestUploadFilesMapperTests
{
    [Fact]
    public async Task success()
    {
        await using var fileStream = File.OpenRead("./Data/hello-world.txt");

        var formFile = new FormFile(fileStream, 0, fileStream.Length, "hello-world.txt", "hello-world.txt");

        var requestUploadFilesMapper = new RequestUploadFilesMapper();

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Form = new FormCollection(null, new FormFileCollection { formFile });

        var result = await requestUploadFilesMapper.MapAsync(httpContext, null, CancellationToken.None);

        result.ShouldBeSuccessWithValueAssertion(x =>
        {
            x.Files.Count().Should().Be(1);

            var formFile = x.Files.Single();

            formFile.Name.Should().Be("hello-world.txt");
            formFile.FileName.Should().Be("hello-world.txt");
            formFile.Length.Should().Be(fileStream.Length);
        });
    }

    [Fact]
    public async Task failure()
    {
        var requestUploadFilesMapper = new RequestUploadFilesMapper();

        var httpContext = new DefaultHttpContext();

        var result = await requestUploadFilesMapper.MapAsync(httpContext, null, CancellationToken.None);

        result.ShouldBeFailureWithErrorContaining("Failed to read upload files");
    }
}