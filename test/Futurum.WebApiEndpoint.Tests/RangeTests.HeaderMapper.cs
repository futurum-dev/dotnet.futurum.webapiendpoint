using Futurum.Test.Option;

using Microsoft.AspNetCore.Http;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class RangeHeaderMapperTests
{
    [Theory]
    [InlineData(0, 10)]
    [InlineData(5, 15)]
    public void is_present(int from, int to)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Add("Range",$"bytes={from}-{to}");

        var range = RangeHeaderMapper.Map(httpContext);
        
        range.ShouldBeHasValueWithValueEquivalentTo(new Range(from, to));
    }
}