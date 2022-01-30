using Futurum.Core.Linq;
using Futurum.Test.Result;

using Microsoft.AspNetCore.Http;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class RequestUploadFilesMapperTests
{
    [Fact]
    public async Task Map()
    {
        await using var fileStream = File.OpenRead("./Data/hello-world.txt");
        
        var formFile = new FormFile(fileStream, 0, fileStream.Length, "hello-world.txt", "hello-world.txt");

        var result = new RequestUploadFilesMapper<object>().Map(new DefaultHttpContext(), new RequestUploadFilesDto(EnumerableExtensions.Return(formFile)));

        result.ShouldBeSuccessWithValueEquivalentTo(new RequestUploadFiles<object>(EnumerableExtensions.Return(formFile)));
    }
}