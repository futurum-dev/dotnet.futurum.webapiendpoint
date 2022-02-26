using FluentAssertions;

using Microsoft.AspNetCore.Http;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseDataCollectionMapperTests
{
    [Fact]
    public void Map()
    {
        var numbers = Enumerable.Range(0, 100)
                                .ToList();

        var result = new ResponseDataCollectionMapper<object, int, int, DataMapper>(new DataMapper())
            .Map(new DefaultHttpContext(), new ResponseDataCollection<object, int>(numbers));

        result.Should().BeEquivalentTo(new ResponseDataCollectionDto<int>(numbers.Select(x => x * 2).ToList()));
    }

    private class DataMapper : IWebApiEndpointResponseDataMapper<int, int>
    {
        public int Map(int data) =>
            data * 2;
    }
}