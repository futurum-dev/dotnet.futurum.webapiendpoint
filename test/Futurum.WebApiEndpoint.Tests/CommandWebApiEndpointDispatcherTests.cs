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

public class CommandWebApiEndpointDispatcherTests
{
    private readonly ITestOutputHelper _output;

    public CommandWebApiEndpointDispatcherTests(ITestOutputHelper output)
    {
        _output = output;
    }

    public class Validator : AbstractValidator<CommandDto>
    {
    }

    public record CommandDto;

    public record Command;

    public record ResponseDto;

    public record Response;

    private static readonly MetadataRouteDefinition MetadataRouteDefinition =
        new(MetadataRouteHttpMethod.Post, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, Option<Action<RouteHandlerBuilder>>.None, null);

    public class CommandWithRequestWithResponse
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
            mocker.Use<IWebApiEndpointRequestValidation<CommandDto>>(new WebApiEndpointRequestValidation<CommandDto>(EnumerableExtensions.Return(new Validator())));
            mocker.Use<IWebApiEndpointRequestMapper<CommandDto, Command>>(new Mapper());
            mocker.Use<IWebApiEndpointResponseMapper<Response, ResponseDto>>(new Mapper());

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(CommandDto), typeof(ResponseDto), typeof(ApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Command, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>));
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
            var metadataDefinition = new MetadataDefinition(MetadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition);

            var middlewareExecutor = new DisabledWebApiEndpointMiddlewareExecutor<Command, Response>();

            var commandWebApiEndpointDispatcher = mocker.CreateInstance<CommandWebApiEndpointDispatcher<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>>();

            return await commandWebApiEndpointDispatcher.ExecuteAsync(metadataDefinition, httpContext, middlewareExecutor, apiEndpoint, CancellationToken.None);
        }

        private class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
        {
            private readonly bool _isSuccess;

            public const string ErrorMessage = "ERROR-MESSAGE";

            public ApiEndpoint(bool isSuccess)
            {
                _isSuccess = isSuccess;
            }

            public bool WasCalled { get; private set; }

            protected override Task<Result<Response>> ExecuteAsync(Command query, CancellationToken cancellationToken)
            {
                WasCalled = true;

                return _isSuccess
                    ? new Response().ToResultOkAsync()
                    : Result.FailAsync<Response>(ErrorMessage);
            }
        }

        public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>, IWebApiEndpointResponseMapper<Response, ResponseDto>
        {
            public Result<Command> Map(HttpContext httpContext, CommandDto dto) =>
                new Command().ToResultOk();

            public Result<ResponseDto> Map(Response domain) =>
                new ResponseDto().ToResultOk();
        }
    }

    public class CommandWithoutRequestWithResponse
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
            mocker.Use<IWebApiEndpointRequestValidation<CommandDto>>(new WebApiEndpointRequestValidation<CommandDto>(EnumerableExtensions.Return(new Validator())));
            mocker.Use<IWebApiEndpointRequestMapper<Command>>(new Mapper());
            mocker.Use<IWebApiEndpointResponseMapper<Response, ResponseDto>>(new Mapper());

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(EmptyRequestDto), typeof(ResponseDto), typeof(ApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<ResponseDto, Command, Response, Mapper, Mapper>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Command, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<ResponseDto, Command, Response, Mapper, Mapper>));
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
            var metadataDefinition = new MetadataDefinition(MetadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition);

            var middlewareExecutor = new DisabledWebApiEndpointMiddlewareExecutor<Command, Response>();

            var commandWebApiEndpointDispatcher = mocker.CreateInstance<CommandWebApiEndpointDispatcher<ResponseDto, Command, Response, Mapper, Mapper>>();

            return await commandWebApiEndpointDispatcher.ExecuteAsync(metadataDefinition, httpContext, middlewareExecutor, apiEndpoint, CancellationToken.None);
        }

        private class ApiEndpoint : CommandWebApiEndpoint.WithRequest<Command>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
        {
            private readonly bool _isSuccess;

            public const string ErrorMessage = "ERROR-MESSAGE";

            public ApiEndpoint(bool isSuccess)
            {
                _isSuccess = isSuccess;
            }

            public bool WasCalled { get; private set; }

            protected override Task<Result<Response>> ExecuteAsync(Command query, CancellationToken cancellationToken)
            {
                WasCalled = true;

                return _isSuccess
                    ? new Response().ToResultOkAsync()
                    : Result.FailAsync<Response>(ErrorMessage);
            }
        }

        public class Mapper : IWebApiEndpointRequestMapper<Command>, IWebApiEndpointResponseMapper<Response, ResponseDto>
        {
            public Result<Command> Map(HttpContext httpContext) =>
                new Command().ToResultOk();

            public Result<ResponseDto> Map(Response domain) =>
                new ResponseDto().ToResultOk();
        }
    }

    public class CommandWithoutResponse
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
            mocker.Use<IWebApiEndpointRequestValidation<CommandDto>>(new WebApiEndpointRequestValidation<CommandDto>(EnumerableExtensions.Return(new Validator())));
            mocker.Use<IWebApiEndpointRequestMapper<CommandDto, Command>>(new Mapper());

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(CommandDto), typeof(EmptyResponseDto), typeof(ApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<CommandDto, Command, Mapper>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Command, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<CommandDto, Command, Mapper>));
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
            var metadataDefinition = new MetadataDefinition(MetadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition);

            var middlewareExecutor = new DisabledWebApiEndpointMiddlewareExecutor<Command, Unit>();

            var commandWebApiEndpointDispatcher = mocker.CreateInstance<CommandWebApiEndpointDispatcher<CommandDto, Command, Mapper>>();

            return await commandWebApiEndpointDispatcher.ExecuteAsync(metadataDefinition, httpContext, middlewareExecutor, apiEndpoint, CancellationToken.None);
        }

        private class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithoutResponse.WithMapper<Mapper>
        {
            private readonly bool _isSuccess;

            public const string ErrorMessage = "ERROR-MESSAGE";

            public ApiEndpoint(bool isSuccess)
            {
                _isSuccess = isSuccess;
            }

            public bool WasCalled { get; private set; }

            protected override Task<Result> ExecuteAsync(Command query, CancellationToken cancellationToken)
            {
                WasCalled = true;

                return _isSuccess
                    ? Result.OkAsync()
                    : Result.FailAsync(ErrorMessage);
            }
        }

        public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
        {
            public Result<Command> Map(HttpContext httpContext, CommandDto dto) =>
                new Command().ToResultOk();
        }
    }

    public class CommandWithoutRequestWithoutResponse
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
            mocker.Use<IWebApiEndpointRequestMapper<Command>>(new Mapper());


            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(EmptyRequestDto), typeof(EmptyResponseDto), typeof(ApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<Command, Mapper>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Command, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<Command, Mapper>));
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
            var metadataDefinition = new MetadataDefinition(MetadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition);

            var middlewareExecutor = new DisabledWebApiEndpointMiddlewareExecutor<Command, Unit>();

            var commandWebApiEndpointDispatcher = mocker.CreateInstance<CommandWebApiEndpointDispatcher<Command, Mapper>>();

            return await commandWebApiEndpointDispatcher.ExecuteAsync(metadataDefinition, httpContext, middlewareExecutor, apiEndpoint, CancellationToken.None);
        }

        private class ApiEndpoint : CommandWebApiEndpoint.WithRequest<Command>.WithoutResponse.WithMapper<Mapper>
        {
            private readonly bool _isSuccess;

            public const string ErrorMessage = "ERROR-MESSAGE";

            public ApiEndpoint(bool isSuccess)
            {
                _isSuccess = isSuccess;
            }

            public bool WasCalled { get; private set; }

            protected override Task<Result> ExecuteAsync(Command query, CancellationToken cancellationToken)
            {
                WasCalled = true;

                return _isSuccess
                    ? Result.OkAsync()
                    : Result.FailAsync(ErrorMessage);
            }
        }

        public class Mapper : IWebApiEndpointRequestMapper<Command>
        {
            public Result<Command> Map(HttpContext httpContext) =>
                new Command().ToResultOk();
        }
    }
}