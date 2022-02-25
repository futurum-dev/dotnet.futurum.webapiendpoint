using FluentAssertions;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseAsyncEnumerableTests
{
    [Fact]
    public void ToApiEndpoint()
    {
        var numbers = Enumerable.Range(0, 100)
                                .ToList();

        var nonGeneric = new ResponseAsyncEnumerable<int>(AsyncEnumerable(numbers));

        var generic = nonGeneric.ToApiEndpoint<object>();

        generic.Data.Should().BeSameAs(nonGeneric.Data);

        async IAsyncEnumerable<int> AsyncEnumerable(IEnumerable<int> numbers)
        {
            await Task.Yield();

            foreach (var number in numbers)
            {
                yield return number;
            }
        }
    }
}