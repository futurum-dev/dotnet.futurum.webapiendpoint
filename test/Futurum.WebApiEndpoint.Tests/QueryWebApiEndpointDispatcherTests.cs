using System.Text.Json;

using FluentAssertions;

using FluentValidation;

using Futurum.Core.Functional;
using Futurum.Core.Linq;
using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.Test.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Internal.Dispatcher;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Moq.AutoMock;

using Xunit;
using Xunit.Abstractions;

namespace Futurum.WebApiEndpoint.Tests;

public class QueryWebApiEndpointDispatcherTests
{
    private readonly ITestOutputHelper _output;

    public QueryWebApiEndpointDispatcherTests(ITestOutputHelper output)
    {
        _output = output;
    }

    public class Validator : AbstractValidator<QueryDto>
    {
    }

    public record QueryDto;

    public record Query;

    public record ResponseDto;

    public record Response;

    private static readonly MetadataRouteDefinition MetadataRouteDefinition =
        new(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, false, Option<Action<RouteHandlerBuilder>>.None, null);

    public class QueryWithoutRequestWithResponse
    {
        [Fact]
        public async Task success()
        {
            var apiEndpoint = new ApiEndpoint(true);
            var httpContext = new DefaultHttpContext();
            var result = await TestRunner(httpContext, apiEndpoint);

            result.ShouldBeSuccess();

            apiEndpoint.WasCalled.Should().BeTrue();

            httpContext.Response.StatusCode.Should().Be(MetadataRouteDefinition.SuccessStatusCode);
        }

        [Fact]
        public async Task failure()
        {
            var apiEndpoint = new ApiEndpoint(false);

            var httpContext = new DefaultHttpContext();
            httpContext.Response.Body = new MemoryStream();

            var result = await TestRunner(httpContext, apiEndpoint);

            result.ShouldBeSuccess();

            apiEndpoint.WasCalled.Should().BeTrue();

            httpContext.Response.StatusCode.Should().Be(MetadataRouteDefinition.FailedStatusCode);

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            using var streamReader = new StreamReader(httpContext.Response.Body);
            var requestBody = await streamReader.ReadToEndAsync();

            var resultErrorStructure = JsonSerializer.Deserialize<ResultErrorStructure>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

            resultErrorStructure.Message.Should().Be(ApiEndpoint.ErrorMessage);
            resultErrorStructure.Children.Should().BeEmpty();
        }

        private static async Task<Result> TestRunner(HttpContext httpContext, ApiEndpoint apiEndpoint)
        {
            var mocker = new AutoMocker();
            mocker.Use<IWebApiEndpointHttpContextDispatcher>(new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions())));
            mocker.Use<IWebApiEndpointRequestMapper<Query>>(new Mapper());
            mocker.Use<IWebApiEndpointResponseMapper<Response, ResponseDto>>(new Mapper());

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(EmptyRequestDto), typeof(ResponseDto), typeof(ApiEndpoint),
                                                                    typeof(IQueryWebApiEndpoint<ResponseDto, Query, Response>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Query, Response>),
                                                                    typeof(QueryWebApiEndpointDispatcher<ResponseDto, Query, Response>));
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
            var metadataDefinition = new MetadataDefinition(MetadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition);

            var middlewareExecutor = new DisabledWebApiEndpointMiddlewareExecutor<Query, Response>();

            var queryWebApiEndpointDispatcher = mocker.CreateInstance<QueryWebApiEndpointDispatcher<ResponseDto, Query, Response>>();

            return await queryWebApiEndpointDispatcher.ExecuteAsync(metadataDefinition, httpContext, middlewareExecutor, apiEndpoint, CancellationToken.None);
        }

        private class ApiEndpoint : QueryWebApiEndpoint.WithRequest<Query>.WithResponse<ResponseDto, Response>
        {
            private readonly bool _isSuccess;

            public const string ErrorMessage = "ERROR-MESSAGE";

            public ApiEndpoint(bool isSuccess)
            {
                _isSuccess = isSuccess;
            }

            public bool WasCalled { get; private set; }

            protected override Task<Result<Response>> ExecuteAsync(Query query, CancellationToken cancellationToken)
            {
                WasCalled = true;


                return _isSuccess
                    ? new Response().ToResultOkAsync()
                    : Result.FailAsync<Response>(ErrorMessage);
            }
        }

        public class Mapper : IWebApiEndpointRequestMapper<Query>, IWebApiEndpointResponseMapper<Response, ResponseDto>
        {
            public Result<Query> Map(HttpContext httpContext) =>
                new Query().ToResultOk();

            public Result<ResponseDto> Map(Response domain) =>
                new ResponseDto().ToResultOk();
        }
    }

    public class QueryWithoutRequestDto
    {
        [Fact]
        public async Task success()
        {
            var apiEndpoint = new ApiEndpoint(true);
            var httpContext = new DefaultHttpContext();
            var result = await TestRunner(httpContext, apiEndpoint);

            result.ShouldBeSuccess();

            apiEndpoint.WasCalled.Should().BeTrue();

            httpContext.Response.StatusCode.Should().Be(MetadataRouteDefinition.SuccessStatusCode);
        }

        [Fact]
        public async Task failure()
        {
            var apiEndpoint = new ApiEndpoint(false);

            var httpContext = new DefaultHttpContext();
            httpContext.Response.Body = new MemoryStream();

            var result = await TestRunner(httpContext, apiEndpoint);

            result.ShouldBeSuccess();

            apiEndpoint.WasCalled.Should().BeTrue();

            httpContext.Response.StatusCode.Should().Be(MetadataRouteDefinition.FailedStatusCode);

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            using var streamReader = new StreamReader(httpContext.Response.Body);
            var requestBody = await streamReader.ReadToEndAsync();

            var resultErrorStructure = JsonSerializer.Deserialize<ResultErrorStructure>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

            resultErrorStructure.Message.Should().Be(ApiEndpoint.ErrorMessage);
            resultErrorStructure.Children.Should().BeEmpty();
        }

        private static async Task<Result> TestRunner(HttpContext httpContext, ApiEndpoint apiEndpoint)
        {
            var mocker = new AutoMocker();
            mocker.Use<IWebApiEndpointHttpContextDispatcher>(new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions())));
            mocker.Use<IWebApiEndpointRequestValidation<QueryDto>>(new WebApiEndpointRequestValidation<QueryDto>(EnumerableExtensions.Return(new Validator())));
            mocker.Use<IWebApiEndpointRequestMapper<QueryDto, Query>>(new Mapper());
            mocker.Use<IWebApiEndpointResponseMapper<Response, ResponseDto>>(new Mapper());

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(QueryDto), typeof(ResponseDto), typeof(ApiEndpoint),
                                                                    typeof(IQueryWebApiEndpoint<QueryDto, ResponseDto, Query, Response>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Query, Response>),
                                                                    typeof(QueryWebApiEndpointDispatcher<QueryDto, ResponseDto, Query, Response>));
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
            var metadataDefinition = new MetadataDefinition(MetadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition);

            var middlewareExecutor = new DisabledWebApiEndpointMiddlewareExecutor<Query, Response>();

            var queryWebApiEndpointDispatcher = mocker.CreateInstance<QueryWebApiEndpointDispatcher<QueryDto, ResponseDto, Query, Response>>();

            return await queryWebApiEndpointDispatcher.ExecuteAsync(metadataDefinition, httpContext, middlewareExecutor, apiEndpoint, CancellationToken.None);
        }

        private class ApiEndpoint : QueryWebApiEndpoint.WithRequest<QueryDto, Query>.WithResponse<ResponseDto, Response>
        {
            private readonly bool _isSuccess;

            public const string ErrorMessage = "ERROR-MESSAGE";

            public ApiEndpoint(bool isSuccess)
            {
                _isSuccess = isSuccess;
            }

            public bool WasCalled { get; private set; }

            protected override Task<Result<Response>> ExecuteAsync(Query query, CancellationToken cancellationToken)
            {
                WasCalled = true;

                return _isSuccess
                    ? new Response().ToResultOkAsync()
                    : Result.FailAsync<Response>(ErrorMessage);
            }
        }

        public class Mapper : IWebApiEndpointRequestMapper<QueryDto, Query>, IWebApiEndpointResponseMapper<Response, ResponseDto>
        {
            public Result<Query> Map(HttpContext httpContext, QueryDto dto) =>
                new Query().ToResultOk();

            public Result<ResponseDto> Map(Response domain) =>
                new ResponseDto().ToResultOk();
        }
    }

    public class QueryWithoutRequest
    {
        [Fact]
        public async Task success()
        {
            var apiEndpoint = new ApiEndpoint(true);
            var httpContext = new DefaultHttpContext();
            var result = await TestRunner(httpContext, apiEndpoint);

            result.ShouldBeSuccess();

            apiEndpoint.WasCalled.Should().BeTrue();

            httpContext.Response.StatusCode.Should().Be(MetadataRouteDefinition.SuccessStatusCode);
        }

        [Fact]
        public async Task failure()
        {
            var apiEndpoint = new ApiEndpoint(false);

            var httpContext = new DefaultHttpContext();
            httpContext.Response.Body = new MemoryStream();

            var result = await TestRunner(httpContext, apiEndpoint);

            result.ShouldBeSuccess();

            apiEndpoint.WasCalled.Should().BeTrue();

            httpContext.Response.StatusCode.Should().Be(MetadataRouteDefinition.FailedStatusCode);

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            using var streamReader = new StreamReader(httpContext.Response.Body);
            var requestBody = await streamReader.ReadToEndAsync();

            var resultErrorStructure = JsonSerializer.Deserialize<ResultErrorStructure>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

            resultErrorStructure.Message.Should().Be(ApiEndpoint.ErrorMessage);
            resultErrorStructure.Children.Should().BeEmpty();
        }

        private static async Task<Result> TestRunner(HttpContext httpContext, ApiEndpoint apiEndpoint)
        {
            var mocker = new AutoMocker();
            mocker.Use<IWebApiEndpointHttpContextDispatcher>(new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions())));
            mocker.Use<IWebApiEndpointResponseMapper<Response, ResponseDto>>(new Mapper());

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(EmptyRequestDto), typeof(ResponseDto), typeof(ApiEndpoint),
                                                                    typeof(IQueryWebApiEndpoint<ResponseDto, Response>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Unit, Response>),
                                                                    typeof(QueryWebApiEndpointDispatcher<ResponseDto, Response>));
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
            var metadataDefinition = new MetadataDefinition(MetadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition);

            var middlewareExecutor = new DisabledWebApiEndpointMiddlewareExecutor<Unit, Response>();

            var queryWebApiEndpointDispatcher = mocker.CreateInstance<QueryWebApiEndpointDispatcher<ResponseDto, Response>>();

            return await queryWebApiEndpointDispatcher.ExecuteAsync(metadataDefinition, httpContext, middlewareExecutor, apiEndpoint, CancellationToken.None);
        }

        private class ApiEndpoint : QueryWebApiEndpoint.WithoutRequest.WithResponse<ResponseDto, Response>
        {
            private readonly bool _isSuccess;

            public const string ErrorMessage = "ERROR-MESSAGE";

            public ApiEndpoint(bool isSuccess)
            {
                _isSuccess = isSuccess;
            }


            public bool WasCalled { get; private set; }

            protected override Task<Result<Response>> ExecuteAsync(CancellationToken cancellationToken)
            {
                WasCalled = true;

                return _isSuccess
                    ? new Response().ToResultOkAsync()
                    : Result.FailAsync<Response>(ErrorMessage);
            }
        }

        public class Mapper : IWebApiEndpointResponseMapper<Response, ResponseDto>
        {
            public Result<ResponseDto> Map(Response domain) =>
                new ResponseDto().ToResultOk();
        }
    }
}