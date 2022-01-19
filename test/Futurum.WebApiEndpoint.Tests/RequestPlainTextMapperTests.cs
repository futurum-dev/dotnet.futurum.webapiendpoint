using Futurum.Test.Result;

using Microsoft.AspNetCore.Http;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class RequestPlainTextMapperTests
{
    [Fact]
    public void Map()
    {
        var input = Guid.NewGuid().ToString();

        var result = new RequestPlainTextMapper<object>().Map(new DefaultHttpContext(), new RequestPlainTextDto(input));

        result.ShouldBeSuccessWithValueEquivalentTo(new RequestPlainText<object>(input));
    }
}