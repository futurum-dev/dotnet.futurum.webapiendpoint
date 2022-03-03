using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Admin;

public static class GetUsers
{
    public record UserDto(string FirstName, string LastName);

    public record User(string FirstName, string LastName);

    public class ApiEndpoint : QueryWebApiEndpoint.NoRequest.ResponseDataCollection<UserDto, User>.Mapper<Mapper>
    {
        protected override Task<Result<ResponseDataCollection<User>>> ExecuteAsync(CancellationToken cancellationToken) =>
            Enumerable.Range(0, 10)
                      .Select(i => new User($"First{i}", $"Last{i}"))
                      .ToResponseDataCollectionAsync();
    }

    public class Mapper : IWebApiEndpointResponseDataMapper<User, UserDto>
    {
        public UserDto Map(User data) =>
            new(data.FirstName, data.LastName);
    }
}