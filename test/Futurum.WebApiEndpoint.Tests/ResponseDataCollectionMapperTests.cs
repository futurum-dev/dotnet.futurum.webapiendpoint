using Futurum.Test.Result;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseDataCollectionMapperTests
{
    [Fact]
    public void Map()
    {
        var numbers = Enumerable.Range(0, 100)
                                .ToList();

        var result = new ResponseDataCollectionMapper<object, int, int, DataMapper>(new DataMapper()).Map(new ResponseDataCollection<object, int>(numbers));

        result.ShouldBeSuccessWithValueEquivalentTo(new ResponseDataCollectionDto<int>(numbers.Select(x => x * 2).ToList()));
    }

    private class DataMapper : IWebApiEndpointDataMapper<int, int>
    {
        public int Map(int data) =>
            data * 2;
    }
}