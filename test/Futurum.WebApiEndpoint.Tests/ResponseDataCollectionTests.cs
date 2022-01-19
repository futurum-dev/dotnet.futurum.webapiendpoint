using FluentAssertions;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseDataCollectionTests
{
    [Fact]
    public void ToApiEndpoint()
    {
        var numbers = Enumerable.Range(0, 100)
                                .ToList();

        var nonGeneric = new ResponseDataCollection<int>(numbers);

        var generic = nonGeneric.ToApiEndpoint<object>();

        nonGeneric.Data.Should().BeSameAs(generic.Data);
    }
}