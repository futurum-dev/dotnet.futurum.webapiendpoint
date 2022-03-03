using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Blog;

public static class BlogGetAsyncEnumerable
{
    public class ApiEndpoint : QueryWebApiEndpoint.NoRequest.ResponseAsyncEnumerable<BlogDto, Blog>.Mapper<BlogDataMapper>
    {
        private readonly IBlogStorageBroker _storageBroker;

        public ApiEndpoint(IBlogStorageBroker storageBroker)
        {
            _storageBroker = storageBroker;
        }

        protected override async Task<Result<ResponseAsyncEnumerable<Blog>>> ExecuteAsync(CancellationToken cancellationToken) =>
            _storageBroker.GetAsAsyncEnumerable()
                          .ToResponseAsyncEnumerable();
    }
}