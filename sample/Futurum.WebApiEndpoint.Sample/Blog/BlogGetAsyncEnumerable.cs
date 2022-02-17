using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Blog;

public static class BlogGetAsyncEnumerable
{
    public class ApiEndpoint : QueryWebApiEndpoint.WithoutRequest.WithResponseAsyncEnumerable<ApiEndpoint, BlogDto, Blog>.WithMapper<BlogDataMapper>
    {
        private readonly IBlogStorageBroker _storageBroker;

        public ApiEndpoint(IBlogStorageBroker storageBroker)
        {
            _storageBroker = storageBroker;
        }

        protected override Task<Result<ResponseAsyncEnumerable<Blog>>> ExecuteAsync(CancellationToken cancellationToken)
        {
            return _storageBroker.GetAsync()
                                 .MapAsync(xs => new ResponseAsyncEnumerable<Blog>(AsyncEnumerable(xs)));

            async IAsyncEnumerable<Blog> AsyncEnumerable(IEnumerable<Blog> dtos)
            {
                await Task.Yield();

                foreach (var dto in dtos)
                {
                    yield return dto;
                }
            }
        }
    }
}