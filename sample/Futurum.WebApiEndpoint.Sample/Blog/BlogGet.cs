using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Blog;

public static class BlogGet
{
    public class ApiEndpoint : QueryWebApiEndpoint.NoRequest.ResponseDataCollection<BlogDto, Blog>.Mapper<BlogDataMapper>
    {
        private readonly IBlogStorageBroker _storageBroker;

        public ApiEndpoint(IBlogStorageBroker storageBroker)
        {
            _storageBroker = storageBroker;
        }

        protected override Task<Result<ResponseDataCollection<Blog>>> ExecuteAsync(CancellationToken cancellationToken) =>
            _storageBroker.GetAsync()
                          .ToResponseDataCollectionAsync();
    }
}