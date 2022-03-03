using System.Text;

using Futurum.Test.Result;

using Microsoft.AspNetCore.Http;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class RequestPlainTextMapperTests
{
    [Fact]
    public async Task Map()
    {
        var message = Guid.NewGuid().ToString();

        var requestPlainTextMapper = new RequestPlainTextMapper();

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Body = new MemoryStream(Encoding.Default.GetBytes(message));

        var result = await requestPlainTextMapper.MapAsync(httpContext, null, CancellationToken.None);

        result.ShouldBeSuccessWithValue(new RequestPlainText(message));
    }
}