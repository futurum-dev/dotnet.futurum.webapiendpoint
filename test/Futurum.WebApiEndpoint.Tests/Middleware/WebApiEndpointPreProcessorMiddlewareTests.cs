using FluentAssertions;

using Futurum.Core.Result;
using Futurum.Test.Result;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

using Microsoft.AspNetCore.Http;

using Xunit;
using Xunit.Abstractions;

namespace Futurum.WebApiEndpoint.Tests.Middleware;

public class WebApiEndpointPreProcessorMiddlewareTests
{
    private readonly ITestOutputHelper _output;

    private const string ErrorMessage = "ERROR_MESSAGE";

    public WebApiEndpointPreProcessorMiddlewareTests(ITestOutputHelper output)
    {
        _output = output;
    }

    public record CommandDto;

    public record Command;

    public record ResponseDto;

    public record Response;

    private class SuccessApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
    {
        private readonly Action _action;

        public SuccessApiEndpoint(Action action)
        {
            _action = action;
        }

        protected override Task<Result<Response>> ExecuteAsync(Command query, CancellationToken cancellationToken)
        {
            _action();

            return new Response().ToResultOkAsync();
        }
    }

    private class FailureApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
    {
        private readonly Action _action;

        public FailureApiEndpoint(Action action)
        {
            _action = action;
        }

        protected override Task<Result<Response>> ExecuteAsync(Command query, CancellationToken cancellationToken)
        {
            _action();

            return Result.FailAsync<Response>(ErrorMessage);
        }
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>, IWebApiEndpointResponseDtoMapper<Response, ResponseDto>
    {
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CommandDto dto, CancellationToken cancellationToken) =>
            throw new NotImplementedException();

        public ResponseDto Map(Response domain) =>
            throw new NotImplementedException();
    }

    public class SuccessMiddleware<TRequest, TResponse> : IWebApiEndpointPreProcessorMiddleware<TRequest>
    {
        private readonly Action _action;

        public SuccessMiddleware(Action action)
        {
            _action = action;
        }

        public Task<Result> ExecuteAsync(HttpContext httpContext, TRequest request, CancellationToken cancellationToken)
        {
            _action();

            return Result.OkAsync();
        }
    }

    public class FailureMiddleware<TRequest, TResponse> : IWebApiEndpointPreProcessorMiddleware<TRequest>
    {
        private readonly Action _action;

        public FailureMiddleware(Action action)
        {
            _action = action;
        }

        public Task<Result> ExecuteAsync(HttpContext httpContext, TRequest request, CancellationToken cancellationToken)
        {
            _action();

            return Result.FailAsync(ErrorMessage);
        }
    }

    [Fact]
    public async Task when_there_is_no_middleware_then_the_apiEndpoint_is_still_called()
    {
        var apiEndpointWasCalled = false;

        var apiEndpoint = new SuccessApiEndpoint(() =>
        {
            apiEndpointWasCalled.Should().BeFalse();

            apiEndpointWasCalled = true;
        });

        apiEndpointWasCalled.Should().BeFalse();

        var middlewares = new IWebApiEndpointPreProcessorMiddleware<Command>[] { };
        var middlewareExecutor = new WebApiEndpointPreProcessorMiddleware<Command, Response>(middlewares);

        var command = new Command();

        var httpContext = new DefaultHttpContext();

        var result = await middlewareExecutor.ExecuteAsync(httpContext, command, (c, ct) => apiEndpoint.ExecuteCommandAsync(c, ct), CancellationToken.None);

        result.ShouldBeSuccess();
        
        apiEndpointWasCalled.Should().BeTrue();
    }

    [Fact]
    public async Task called_in_the_correct_order()
    {
        var middleware1WasCalled = false;
        var middleware2WasCalled = false;
        var apiEndpointWasCalled = false;

        var testMiddleware1 = new SuccessMiddleware<Command, Response>(() =>
        {
            middleware1WasCalled.Should().BeFalse();
            middleware2WasCalled.Should().BeFalse();
            apiEndpointWasCalled.Should().BeFalse();

            middleware1WasCalled = true;
        });

        var testMiddleware2 = new SuccessMiddleware<Command, Response>(() =>
        {
            middleware1WasCalled.Should().BeTrue();
            middleware2WasCalled.Should().BeFalse();
            apiEndpointWasCalled.Should().BeFalse();

            middleware2WasCalled = true;
        });

        var apiEndpoint = new SuccessApiEndpoint(() =>
        {
            middleware1WasCalled.Should().BeTrue();
            middleware2WasCalled.Should().BeTrue();
            apiEndpointWasCalled.Should().BeFalse();

            apiEndpointWasCalled = true;
        });

        apiEndpointWasCalled.Should().BeFalse();

        var middlewares = new IWebApiEndpointPreProcessorMiddleware<Command>[] { testMiddleware1, testMiddleware2 };
        var middlewareExecutor = new WebApiEndpointPreProcessorMiddleware<Command, Response>(middlewares);

        var command = new Command();

        var httpContext = new DefaultHttpContext();

        var result = await middlewareExecutor.ExecuteAsync(httpContext, command, (c, ct) => apiEndpoint.ExecuteCommandAsync(c, ct), CancellationToken.None);

        result.ShouldBeSuccess();
        
        middleware1WasCalled.Should().BeTrue();
        middleware2WasCalled.Should().BeTrue();
        apiEndpointWasCalled.Should().BeTrue();
    }

    [Fact]
    public async Task when_ApiEndpoint_returns_failure_then_correct_error_returned()
    {
        var middleware1WasCalled = false;
        var middleware2WasCalled = false;
        var apiEndpointWasCalled = false;

        var testMiddleware1 = new SuccessMiddleware<Command, Response>(() =>
        {
            apiEndpointWasCalled.Should().BeFalse();
            middleware1WasCalled.Should().BeFalse();
            middleware2WasCalled.Should().BeFalse();

            middleware1WasCalled = true;
        });

        var testMiddleware2 = new SuccessMiddleware<Command, Response>(() =>
        {
            middleware1WasCalled.Should().BeTrue();
            middleware2WasCalled.Should().BeFalse();
            apiEndpointWasCalled.Should().BeFalse();

            middleware2WasCalled = true;
        });

        var apiEndpoint = new FailureApiEndpoint(() =>
        {
            middleware1WasCalled.Should().BeTrue();
            middleware2WasCalled.Should().BeTrue();
            apiEndpointWasCalled.Should().BeFalse();

            apiEndpointWasCalled = true;
        });

        apiEndpointWasCalled.Should().BeFalse();

        var middlewares = new IWebApiEndpointPreProcessorMiddleware<Command>[] { testMiddleware1, testMiddleware2 };
        var middlewareExecutor = new WebApiEndpointPreProcessorMiddleware<Command, Response>(middlewares);

        var command = new Command();

        var httpContext = new DefaultHttpContext();

        var result = await middlewareExecutor.ExecuteAsync(httpContext, command, (c, ct) => apiEndpoint.ExecuteCommandAsync(c, ct), CancellationToken.None);

        middleware1WasCalled.Should().BeTrue();
        middleware2WasCalled.Should().BeTrue();
        apiEndpointWasCalled.Should().BeTrue();

        result.ShouldBeFailureWithError(ErrorMessage);
    }

    [Fact]
    public async Task when_middleware_returns_failure_then_no_more_called_and_correct_error_returned()
    {
        var apiEndpointWasCalled = false;
        var middleware1WasCalled = false;
        var middleware2WasCalled = false;

        var testMiddleware1 = new FailureMiddleware<Command, Response>(() =>
        {
            middleware1WasCalled.Should().BeFalse();
            middleware2WasCalled.Should().BeFalse();
            apiEndpointWasCalled.Should().BeFalse();

            middleware1WasCalled = true;
        });

        var testMiddleware2 = new SuccessMiddleware<Command, Response>(() =>
        {
            middleware1WasCalled.Should().BeTrue();
            middleware2WasCalled.Should().BeFalse();
            apiEndpointWasCalled.Should().BeFalse();

            middleware2WasCalled = true;
        });

        var apiEndpoint = new FailureApiEndpoint(() =>
        {
            middleware1WasCalled.Should().BeTrue();
            middleware2WasCalled.Should().BeTrue();
            apiEndpointWasCalled.Should().BeFalse();

            apiEndpointWasCalled = true;
        });

        apiEndpointWasCalled.Should().BeFalse();

        var middlewares = new IWebApiEndpointPreProcessorMiddleware<Command>[] { testMiddleware1, testMiddleware2 };
        var middlewareExecutor = new WebApiEndpointPreProcessorMiddleware<Command, Response>(middlewares);

        var command = new Command();

        var httpContext = new DefaultHttpContext();

        var result = await middlewareExecutor.ExecuteAsync(httpContext, command, (c, ct) => apiEndpoint.ExecuteCommandAsync(c, ct), CancellationToken.None);

        middleware1WasCalled.Should().BeTrue();
        middleware2WasCalled.Should().BeFalse();
        apiEndpointWasCalled.Should().BeFalse();

        result.ShouldBeFailureWithError(ErrorMessage);
    }
}