using Futurum.Test.Result;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseAsyncEnumerableMapperTests
{
    [Fact]
    public async Task Map()
    {
        var numbers = Enumerable.Range(0, 100)
                                .ToList();

        var nonGeneric = new ResponseAsyncEnumerable<object, int>(AsyncEnumerable(numbers));

        var result = new ResponseAsyncEnumerableMapper<object, int, int, DataMapper>(new DataMapper()).Map(nonGeneric);

        await result.ShouldBeSuccessWithValueEquivalentToAsync(x => x.AsyncEnumerable,
                                                               new ResponseAsyncEnumerableDto<int>(AsyncEnumerable(numbers)).AsyncEnumerable);
    }

    private class DataMapper : IWebApiEndpointDataMapper<int, int>
    {
        public int Map(int data) =>
            data;
    }

    async IAsyncEnumerable<int> AsyncEnumerable(IEnumerable<int> numbers)
    {
        await Task.Yield();

        foreach (var number in numbers)
        {
            yield return number;
        }
    }
}