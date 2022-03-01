using Futurum.Core.Result;
using Futurum.Test.Result;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseAsyncEnumerableExtensionsTests
{
    [Fact]
    public void Result()
    {
        var numbers = AsyncEnumerable.Range(0, 100);

        var result = numbers.ToResultOk().ToResponseAsyncEnumerable();

        result.ShouldBeSuccessWithValue(new ResponseAsyncEnumerable<int>(numbers));
    }
}