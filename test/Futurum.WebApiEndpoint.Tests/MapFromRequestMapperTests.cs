using Futurum.Core.Result;
using Futurum.Test.Result;
using Futurum.WebApiEndpoint.Internal;

using HttpContextMoq;
using HttpContextMoq.Extensions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using Xunit;
using Xunit.Abstractions;

namespace Futurum.WebApiEndpoint.Tests;

public class MapFromRequestMapperTests
{
    private const string Key = "Id";
    private const string StringValue = "Id";
    private const int IntValue = 0;
    private const long LongValue = 0L;

    private readonly ITestOutputHelper _output;

    public MapFromRequestMapperTests(ITestOutputHelper output)
    {
        _output = output;
    }

    public class MapWithoutDto
    {
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
        }

        public class Form
        {
            public record RequestFormString
            {
                [MapFromForm(Key)] public string Id { get; set; }
            }

            public record RequestFormInt
            {
                [MapFromForm(Key)] public int Id { get; set; }
            }

            public record RequestFormLong
            {
                [MapFromForm(Key)] public long Id { get; set; }
            }

            public class String
            {
                [Fact]
                public void success()
                {
                    var result = TestRunnerMap<RequestFormString>(httpContext => httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues> { { Key, StringValue } }));

                    result.ShouldBeSuccessWithValueEquivalentTo(new RequestFormString { Id = StringValue });
                }

                [Fact]
                public void failure()
                {
                    var result = TestRunnerMap<RequestFormString>(httpContext => httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues> { }));

                    result.ShouldBeFailureWithError($"Unable to get Request Form Parameter - '{Key}'. Request Form Parameters available are ''");
                }
            }

            public class Int
            {
                [Fact]
                public void success()
                {
                    var result = TestRunnerMap<RequestFormInt>(httpContext => httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues> { { Key, IntValue.ToString() } }));

                    result.ShouldBeSuccessWithValueEquivalentTo(new RequestFormInt { Id = IntValue });
                }

                [Fact]
                public void failure()
                {
                    var result = TestRunnerMap<RequestFormInt>(httpContext => httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues> { }));

                    result.ShouldBeFailureWithError($"Unable to get Request Form Parameter - '{Key}'. Request Form Parameters available are ''");
                }
            }

            public class Long
            {
                [Fact]
                public void success()
                {
                    var result = TestRunnerMap<RequestFormLong>(httpContext => httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues> { { Key, LongValue.ToString() } }));

                    result.ShouldBeSuccessWithValueEquivalentTo(new RequestFormLong { Id = LongValue });
                }

                [Fact]
                public void failure()
                {
                    var result = TestRunnerMap<RequestFormLong>(httpContext => httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>()));

                    result.ShouldBeFailureWithError($"Unable to get Request Form Parameter - '{Key}'. Request Form Parameters available are ''");
                }
            }
        }

        private static Result<T> TestRunnerMap<T>(Action<HttpContext>? configureHttpContext = null)
            where T : class
        {
            var mapFromRequestMapper = new MapFromRequestMapper<T>();

            var httpContext = new DefaultHttpContext();
            configureHttpContext?.Invoke(httpContext);

            return MapFromRequestMapper<T>.Map(httpContext);
        }

        private static Result<T> TestRunnerCookieMap<T>(Action<HttpContextMock>? configureHttpContext = null)
            where T : class
        {
            var mapFromRequestMapper = new MapFromRequestMapper<T>();

            var httpContext = new HttpContextMock();
            configureHttpContext?.Invoke(httpContext);

            return MapFromRequestMapper<T>.Map(httpContext);
        }
    }

    public class MapWithDto
    {
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
        }

        public class Form
        {
            public record RequestFormString
            {
                [MapFromForm(Key)] public string Id { get; set; }
            }

            public record RequestFormInt
            {
                [MapFromForm(Key)] public int Id { get; set; }
            }

            public record RequestFormLong
            {
                [MapFromForm(Key)] public long Id { get; set; }
            }

            public class String
            {
                [Fact]
                public void success()
                {
                    var result = TestRunnerMap<RequestFormString>(httpContext => httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues> { { Key, StringValue } }));

                    result.ShouldBeSuccessWithValueEquivalentTo(new RequestFormString { Id = StringValue });
                }

                [Fact]
                public void failure()
                {
                    var result = TestRunnerMap<RequestFormString>(httpContext => httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues> { }));

                    result.ShouldBeFailureWithError($"Unable to get Request Form Parameter - '{Key}'. Request Form Parameters available are ''");
                }
            }

            public class Int
            {
                [Fact]
                public void success()
                {
                    var result = TestRunnerMap<RequestFormInt>(httpContext => httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues> { { Key, IntValue.ToString() } }));

                    result.ShouldBeSuccessWithValueEquivalentTo(new RequestFormInt { Id = IntValue });
                }

                [Fact]
                public void failure()
                {
                    var result = TestRunnerMap<RequestFormInt>(httpContext => httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues> { }));

                    result.ShouldBeFailureWithError($"Unable to get Request Form Parameter - '{Key}'. Request Form Parameters available are ''");
                }
            }

            public class Long
            {
                [Fact]
                public void success()
                {
                    var result = TestRunnerMap<RequestFormLong>(httpContext => httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues> { { Key, LongValue.ToString() } }));

                    result.ShouldBeSuccessWithValueEquivalentTo(new RequestFormLong { Id = LongValue });
                }

                [Fact]
                public void failure()
                {
                    var result = TestRunnerMap<RequestFormLong>(httpContext => httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues> { }));

                    result.ShouldBeFailureWithError($"Unable to get Request Form Parameter - '{Key}'. Request Form Parameters available are ''");
                }
            }
        }
        
        private static Result<T> TestRunnerMap<T>(Action<HttpContext>? configureHttpContext = null)
            where T : class, new()
        {
            var mapFromRequestMapper = new MapFromRequestMapper<T>();

            var httpContext = new DefaultHttpContext();
            configureHttpContext?.Invoke(httpContext);

            var dto = new T();
            return MapFromRequestMapper<T>.Map(httpContext, dto);
        }

        private static Result<T> TestRunnerCookieMap<T>(Action<HttpContextMock>? configureHttpContext = null)
            where T : class, new()
        {
            var mapFromRequestMapper = new MapFromRequestMapper<T>();

            var httpContext = new HttpContextMock();
            configureHttpContext?.Invoke(httpContext);

            var dto = new T();
            return MapFromRequestMapper<T>.Map(httpContext, dto);
        }
    }
}