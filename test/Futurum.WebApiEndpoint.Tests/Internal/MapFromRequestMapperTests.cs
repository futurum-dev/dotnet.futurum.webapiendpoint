using FluentAssertions;

using Futurum.Core.Result;
using Futurum.Test.Result;
using Futurum.WebApiEndpoint.Internal;

using HttpContextMoq;
using HttpContextMoq.Extensions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using Xunit;
using Xunit.Abstractions;

namespace Futurum.WebApiEndpoint.Tests.Internal;

public class MapFromRequestMapperTests
{
    private const string Key = "Id";
    private const string StringValue = "Id";
    private const int IntValue = 0;
    private const long LongValue = 0L;
    private const bool BoolValue = true;
    private static Guid GuidValue = Guid.NewGuid();

    private readonly ITestOutputHelper _output;

    public MapFromRequestMapperTests(ITestOutputHelper output)
    {
        _output = output;
    }

    public class Path
    {
        public record RequestPathString
        {
            [MapFromPath(Key)] public string Id { get; set; }
        }

        public record RequestPathInt
        {
            [MapFromPath(Key)] public int Id { get; set; }
        }

        public record RequestPathLong
        {
            [MapFromPath(Key)] public long Id { get; set; }
        }

        public record RequestPathDateTime
        {
            [MapFromPath(Key)] public System.DateTime Id { get; set; }
        }

        public record RequestPathBool
        {
            [MapFromPath(Key)] public bool Id { get; set; }
        }

        public record RequestPathGuid
        {
            [MapFromPath(Key)] public System.Guid Id { get; set; }
        }

        public class String
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerMap<RequestPathString>(httpContext => httpContext.Request.RouteValues.Add(Key, StringValue));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestPathString { Id = StringValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerMap<RequestPathString>();

                result.ShouldBeFailureWithError($"Unable to get Request Path Parameter - '{Key}'. Request Path Parameters available are ''");
            }
        }

        public class Int
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerMap<RequestPathInt>(httpContext => httpContext.Request.RouteValues.Add(Key, IntValue));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestPathInt { Id = IntValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerMap<RequestPathInt>();

                result.ShouldBeFailureWithError($"Unable to get Request Path Parameter - '{Key}'. Request Path Parameters available are ''");
            }
        }

        public class Long
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerMap<RequestPathLong>(httpContext => httpContext.Request.RouteValues.Add(Key, LongValue));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestPathLong { Id = LongValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerMap<RequestPathLong>();

                result.ShouldBeFailureWithError($"Unable to get Request Path Parameter - '{Key}'. Request Path Parameters available are ''");
            }
        }

        public class DateTime
        {
            [Fact]
            public void success()
            {
                var dateTimeNow = System.DateTime.Now;
                var dateTimeValue = new System.DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, dateTimeNow.Hour, dateTimeNow.Minute, dateTimeNow.Second, dateTimeNow.Kind);
            
                var result = TestRunnerMap<RequestPathDateTime>(httpContext => httpContext.Request.RouteValues.Add(Key, dateTimeValue));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestPathDateTime { Id = dateTimeValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerMap<RequestPathDateTime>();

                result.ShouldBeFailureWithError($"Unable to get Request Path Parameter - '{Key}'. Request Path Parameters available are ''");
            }
        }

        public class Bool
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerMap<RequestPathBool>(httpContext => httpContext.Request.RouteValues.Add(Key, BoolValue));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestPathBool { Id = BoolValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerMap<RequestPathBool>();

                result.ShouldBeFailureWithError($"Unable to get Request Path Parameter - '{Key}'. Request Path Parameters available are ''");
            }
        }

        public class Guid
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerMap<RequestPathGuid>(httpContext => httpContext.Request.RouteValues.Add(Key, GuidValue));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestPathGuid { Id = GuidValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerMap<RequestPathGuid>();

                result.ShouldBeFailureWithError($"Unable to get Request Path Parameter - '{Key}'. Request Path Parameters available are ''");
            }
        }
    }

    public class Query
    {
        public record RequestQueryString
        {
            [MapFromQuery(Key)] public string Id { get; set; }
        }

        public record RequestQueryInt
        {
            [MapFromQuery(Key)] public int Id { get; set; }
        }

        public record RequestQueryLong
        {
            [MapFromQuery(Key)] public long Id { get; set; }
        }

        public record RequestQueryDateTime
        {
            [MapFromQuery(Key)] public System.DateTime Id { get; set; }
        }

        public record RequestQueryBool
        {
            [MapFromQuery(Key)] public bool Id { get; set; }
        }

        public record RequestQueryGuid
        {
            [MapFromQuery(Key)] public System.Guid Id { get; set; }
        }

        public class String
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerMap<RequestQueryString>(httpContext =>
                                                                        httpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues> { { Key, StringValue } }));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestQueryString { Id = StringValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerMap<RequestQueryString>();

                result.ShouldBeFailureWithError($"Unable to get Request Query Parameter - '{Key}'. Request Query Parameters available are ''");
            }
        }

        public class Int
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerMap<RequestQueryInt>(httpContext =>
                                                                     httpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues> { { Key, IntValue.ToString() } }));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestQueryInt { Id = IntValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerMap<RequestQueryInt>();

                result.ShouldBeFailureWithError($"Unable to get Request Query Parameter - '{Key}'. Request Query Parameters available are ''");
            }
        }

        public class Long
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerMap<RequestQueryLong>(httpContext =>
                                                                      httpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues> { { Key, LongValue.ToString() } }));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestQueryLong { Id = LongValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerMap<RequestQueryLong>();

                result.ShouldBeFailureWithError($"Unable to get Request Query Parameter - '{Key}'. Request Query Parameters available are ''");
            }
        }

        public class DateTime
        {
            [Fact]
            public void success()
            {
                var dateTimeNow = System.DateTime.Now;
                var dateTimeValue = new System.DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, dateTimeNow.Hour, dateTimeNow.Minute, dateTimeNow.Second, dateTimeNow.Kind);

                var result = TestRunnerMap<RequestQueryDateTime>(httpContext =>
                                                                     httpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues> { { Key, dateTimeValue.ToString() } }));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestQueryDateTime { Id = dateTimeValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerMap<RequestQueryDateTime>();

                result.ShouldBeFailureWithError($"Unable to get Request Query Parameter - '{Key}'. Request Query Parameters available are ''");
            }
        }

        public class Bool
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerMap<RequestQueryBool>(httpContext =>
                                                                 httpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues> { { Key, BoolValue.ToString() } }));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestQueryBool { Id = BoolValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerMap<RequestQueryBool>();

                result.ShouldBeFailureWithError($"Unable to get Request Query Parameter - '{Key}'. Request Query Parameters available are ''");
            }
        }

        public class Guid
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerMap<RequestQueryGuid>(httpContext =>
                                                                 httpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues> { { Key, GuidValue.ToString() } }));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestQueryGuid { Id = GuidValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerMap<RequestQueryGuid>();

                result.ShouldBeFailureWithError($"Unable to get Request Query Parameter - '{Key}'. Request Query Parameters available are ''");
            }
        }
    }

    public class Header
    {
        public record RequestHeaderString
        {
            [MapFromHeader(Key)] public string Id { get; set; }
        }

        public record RequestHeaderInt
        {
            [MapFromHeader(Key)] public int Id { get; set; }
        }

        public record RequestHeaderLong
        {
            [MapFromHeader(Key)] public long Id { get; set; }
        }

        public record RequestHeaderDateTime
        {
            [MapFromHeader(Key)] public System.DateTime Id { get; set; }
        }

        public record RequestHeaderBool
        {
            [MapFromHeader(Key)] public bool Id { get; set; }
        }

        public record RequestHeaderGuid
        {
            [MapFromHeader(Key)] public System.Guid Id { get; set; }
        }

        public class String
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerMap<RequestHeaderString>(httpContext => httpContext.Request.Headers.Add(Key, StringValue));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestHeaderString { Id = StringValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerMap<RequestHeaderString>();

                result.ShouldBeFailureWithError($"Unable to get Request Header Parameter - '{Key}'. Request Header Parameters available are ''");
            }
        }

        public class Int
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerMap<RequestHeaderInt>(httpContext => httpContext.Request.Headers.Add(Key, IntValue.ToString()));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestHeaderInt { Id = IntValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerMap<RequestHeaderInt>();

                result.ShouldBeFailureWithError($"Unable to get Request Header Parameter - '{Key}'. Request Header Parameters available are ''");
            }
        }

        public class Long
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerMap<RequestHeaderLong>(httpContext => httpContext.Request.Headers.Add(Key, LongValue.ToString()));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestHeaderLong { Id = LongValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerMap<RequestHeaderLong>();

                result.ShouldBeFailureWithError($"Unable to get Request Header Parameter - '{Key}'. Request Header Parameters available are ''");
            }
        }

        public class DateTime
        {
            [Fact]
            public void success()
            {
                var dateTimeNow = System.DateTime.Now;
                var dateTimeValue = new System.DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, dateTimeNow.Hour, dateTimeNow.Minute, dateTimeNow.Second, dateTimeNow.Kind);

                var result = TestRunnerMap<RequestHeaderDateTime>(httpContext => httpContext.Request.Headers.Add(Key, dateTimeValue.ToString()));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestHeaderDateTime { Id = dateTimeValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerMap<RequestHeaderDateTime>();

                result.ShouldBeFailureWithError($"Unable to get Request Header Parameter - '{Key}'. Request Header Parameters available are ''");
            }
        }

        public class Bool
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerMap<RequestHeaderBool>(httpContext => httpContext.Request.Headers.Add(Key, BoolValue.ToString()));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestHeaderBool { Id = BoolValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerMap<RequestHeaderBool>();

                result.ShouldBeFailureWithError($"Unable to get Request Header Parameter - '{Key}'. Request Header Parameters available are ''");
            }
        }

        public class Guid
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerMap<RequestHeaderGuid>(httpContext => httpContext.Request.Headers.Add(Key, GuidValue.ToString()));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestHeaderGuid { Id = GuidValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerMap<RequestHeaderGuid>();

                result.ShouldBeFailureWithError($"Unable to get Request Header Parameter - '{Key}'. Request Header Parameters available are ''");
            }
        }
    }

    public class Cookie
    {
        public record RequestCookieString
        {
            [MapFromCookie(Key)] public string Id { get; set; }
        }

        public record RequestCookieInt
        {
            [MapFromCookie(Key)] public int Id { get; set; }
        }

        public record RequestCookieLong
        {
            [MapFromCookie(Key)] public long Id { get; set; }
        }

        public record RequestCookieDateTime
        {
            [MapFromCookie(Key)] public System.DateTime Id { get; set; }
        }

        public record RequestCookieBool
        {
            [MapFromCookie(Key)] public bool Id { get; set; }
        }

        public record RequestCookieGuid
        {
            [MapFromCookie(Key)] public System.Guid Id { get; set; }
        }

        public class String
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerCookieMap<RequestCookieString>(httpContext => httpContext.SetupRequestCookies(new Dictionary<string, string> { { Key, StringValue } }));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestCookieString { Id = StringValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerCookieMap<RequestCookieString>(httpContext => httpContext.SetupRequestCookies(new Dictionary<string, string> { }));

                result.ShouldBeFailureWithError($"Unable to get Request Cookie Parameter - '{Key}'. Request Cookie Parameters available are ''");
            }
        }

        public class Int
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerCookieMap<RequestCookieInt>(httpContext => httpContext.SetupRequestCookies(new Dictionary<string, string> { { Key, IntValue.ToString() } }));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestCookieInt { Id = IntValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerCookieMap<RequestCookieInt>(httpContext => httpContext.SetupRequestCookies(new Dictionary<string, string> { }));

                result.ShouldBeFailureWithError($"Unable to get Request Cookie Parameter - '{Key}'. Request Cookie Parameters available are ''");
            }
        }

        public class Long
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerCookieMap<RequestCookieLong>(httpContext => httpContext.SetupRequestCookies(new Dictionary<string, string> { { Key, LongValue.ToString() } }));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestCookieLong { Id = LongValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerCookieMap<RequestCookieLong>(httpContext => httpContext.SetupRequestCookies(new Dictionary<string, string> { }));

                result.ShouldBeFailureWithError($"Unable to get Request Cookie Parameter - '{Key}'. Request Cookie Parameters available are ''");
            }
        }

        public class DateTime
        {
            [Fact]
            public void success()
            {
                var dateTimeNow = System.DateTime.Now;
                var dateTimeValue = new System.DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, dateTimeNow.Hour, dateTimeNow.Minute, dateTimeNow.Second, dateTimeNow.Kind);

                var result = TestRunnerCookieMap<RequestCookieDateTime>(httpContext => httpContext.SetupRequestCookies(new Dictionary<string, string> { { Key, dateTimeValue.ToString() } }));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestCookieDateTime { Id = dateTimeValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerCookieMap<RequestCookieDateTime>(httpContext => httpContext.SetupRequestCookies(new Dictionary<string, string> { }));

                result.ShouldBeFailureWithError($"Unable to get Request Cookie Parameter - '{Key}'. Request Cookie Parameters available are ''");
            }
        }

        public class Bool
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerCookieMap<RequestCookieBool>(httpContext => httpContext.SetupRequestCookies(new Dictionary<string, string> { { Key, BoolValue.ToString() } }));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestCookieBool { Id = BoolValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerCookieMap<RequestCookieBool>(httpContext => httpContext.SetupRequestCookies(new Dictionary<string, string> { }));

                result.ShouldBeFailureWithError($"Unable to get Request Cookie Parameter - '{Key}'. Request Cookie Parameters available are ''");
            }
        }

        public class Guid
        {
            [Fact]
            public void success()
            {
                var result = TestRunnerCookieMap<RequestCookieGuid>(httpContext => httpContext.SetupRequestCookies(new Dictionary<string, string> { { Key, GuidValue.ToString() } }));

                result.ShouldBeSuccessWithValueEquivalentTo(new RequestCookieGuid { Id = GuidValue });
            }

            [Fact]
            public void failure()
            {
                var result = TestRunnerCookieMap<RequestCookieGuid>(httpContext => httpContext.SetupRequestCookies(new Dictionary<string, string> { }));

                result.ShouldBeFailureWithError($"Unable to get Request Cookie Parameter - '{Key}'. Request Cookie Parameters available are ''");
            }
        }
    }

    private static Result<T> TestRunnerMap<T>(Action<HttpContext>? configureHttpContext = null)
        where T : class, new()
    {
        var httpContext = new DefaultHttpContext();
        configureHttpContext?.Invoke(httpContext);

        var dto = new T();
        return MapFromRequestMapper<T>.Map(httpContext, dto, CancellationToken.None)
                                      .Map(dto);
    }

    private static Result<T> TestRunnerCookieMap<T>(Action<HttpContextMock>? configureHttpContext = null)
        where T : class, new()
    {
        var httpContext = new HttpContextMock();
        configureHttpContext?.Invoke(httpContext);

        var dto = new T();
        return MapFromRequestMapper<T>.Map(httpContext, dto, CancellationToken.None)
                                      .Map(dto);
    }

    public class when_property_is_readonly_the_fail
    {
        [Fact]
        public void failure()
        {
            var httpContext = new DefaultHttpContext();

            var action = () => MapFromRequestMapper<RequestWithReadonlyProperty>.Map(httpContext, new RequestWithReadonlyProperty(), CancellationToken.None);

            action.Should().Throw<TypeInitializationException>().WithInnerException<InvalidOperationException>()
                  .WithMessage($"Property '{nameof(RequestWithReadonlyProperty.Id)}' on RequestDto type : '{typeof(RequestWithReadonlyProperty).FullName}' is readonly");
        }

        public record RequestWithReadonlyProperty
        {
            [MapFromPath(Key)] public string Id { get; }
        }
    }
}