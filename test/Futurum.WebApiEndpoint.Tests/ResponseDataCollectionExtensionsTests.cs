using Futurum.Core.Result;
using Futurum.Test.Result;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseDataCollectionExtensionsTests
{
    public class ToResponseDataCollectionAsync
    {
        [Fact]
        public async Task IEnumerable()
        {
            var numbers = Enumerable.Range(0, 100);

            var result = await numbers.ToResponseDataCollectionAsync();

            result.ShouldBeSuccessWithValue(new ResponseDataCollection<int>(numbers));
        }
        
        [Fact]
        public async Task Result()
        {
            var numbers = Enumerable.Range(0, 100);

            var result = await numbers.ToResultOk().ToResponseDataCollectionAsync();

            result.ShouldBeSuccessWithValue(new ResponseDataCollection<int>(numbers));
        }
        
        [Fact]
        public async Task ResultAsync()
        {
            var numbers = Enumerable.Range(0, 100);

            var result = await numbers.ToResultOkAsync().ToResponseDataCollectionAsync();

            result.ShouldBeSuccessWithValue(new ResponseDataCollection<int>(numbers));
        }
    }
}