using System.Text.Json;

using FluentAssertions;

using Futurum.WebApiEndpoint.Internal;

using Microsoft.AspNetCore.Http;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.Internal;

public class MapFromRequestMultipartMapperTests
{
    public class when_property_is_readonly_the_fail
    {
        [Fact]
        public void failure()
        {
            var httpContext = new DefaultHttpContext();

            var action = () => MapFromRequestMultipartMapper<RequestWithReadonlyProperty>.MapAsync(httpContext, new JsonSerializerOptions(), new RequestWithReadonlyProperty(), CancellationToken.None);

            action.Should().ThrowAsync<TypeInitializationException>().WithInnerException<TypeInitializationException, InvalidOperationException>()
                  .WithMessage($"Property '{nameof(RequestWithReadonlyProperty.Id)}' on RequestDto type : '{typeof(RequestWithReadonlyProperty).FullName}' is readonly");
        }

        public record RequestWithReadonlyProperty
        {
            [MapFromMultipartJson(1)] public string Id { get; }
        }
    }
}