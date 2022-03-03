using Futurum.Core.Result;
using Futurum.Test.Result;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseEmptyExtensionsTests
{
    public const string ERROR_MESSAGE = "Error Message";
    
    public class Sync
    {
        public class non_generic
        {
            [Fact]
            public async Task success()
            {
                var inputResult = Result.Ok();

                var result = await inputResult.ToResponseEmptyAsync();
                
                result.ShouldBeSuccessWithValue(ResponseEmpty.Default);
            }
            
            [Fact]
            public async Task failure()
            {
                var inputResult = Result.Fail(ERROR_MESSAGE);

                var result = await inputResult.ToResponseEmptyAsync();
                
                result.ShouldBeFailureWithError(ERROR_MESSAGE);
            }
        }
        
        public class generic
        {
            [Fact]
            public async Task success()
            {
                var inputResult = Result.Ok(10);

                var result = await inputResult.ToResponseEmptyAsync();
                
                result.ShouldBeSuccessWithValue(ResponseEmpty.Default);
            }
            
            [Fact]
            public async Task failure()
            {
                var inputResult = Result.Fail<int>(ERROR_MESSAGE);

                var result = await inputResult.ToResponseEmptyAsync();
                
                result.ShouldBeFailureWithError(ERROR_MESSAGE);
            }
        }
    }
    
    public class Async
    {
        public class non_generic
        {
            [Fact]
            public async Task success()
            {
                var inputResult = Result.OkAsync();

                var result = await inputResult.ToResponseEmptyAsync();
                
                result.ShouldBeSuccessWithValue(ResponseEmpty.Default);
            }
            
            [Fact]
            public async Task failure()
            {
                var inputResult = Result.FailAsync(ERROR_MESSAGE);

                var result = await inputResult.ToResponseEmptyAsync();
                
                result.ShouldBeFailureWithError(ERROR_MESSAGE);
            }
        }
        
        public class generic
        {
            [Fact]
            public async Task success()
            {
                var inputResult = Result.OkAsync(10);

                var result = await inputResult.ToResponseEmptyAsync();
                
                result.ShouldBeSuccessWithValue(ResponseEmpty.Default);
            }
            
            [Fact]
            public async Task failure()
            {
                var inputResult = Result.FailAsync<int>(ERROR_MESSAGE);

                var result = await inputResult.ToResponseEmptyAsync();
                
                result.ShouldBeFailureWithError(ERROR_MESSAGE);
            }
        }
    }
}