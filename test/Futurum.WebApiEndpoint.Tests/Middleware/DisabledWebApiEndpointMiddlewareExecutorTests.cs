using FluentAssertions;

using Futurum.Core.Result;
using Futurum.Test.Result;
using Futurum.WebApiEndpoint.Middleware;

using Microsoft.AspNetCore.Http;

using Xunit;
using Xunit.Abstractions;

namespace Futurum.WebApiEndpoint.Tests.Middleware;

public class DisabledWebApiEndpointMiddlewareExecutorTests
{
    private readonly ITestOutputHelper _output;

    public DisabledWebApiEndpointMiddlewareExecutorTests(ITestOutputHelper output)
    {
        _output = output;
    }

    public record CommandDto;
    public record Command;
    
    public record ResponseDto;
    public record Response;
    
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
            throw new NotImplementedException();

        public Result<ResponseDto> Map(Response domain) =>
            throw new NotImplementedException();
    }

    [Fact]
    public async Task verify_calls_ApiEndpoint_and_returns_correctly_for_success()
    {
        var middlewareExecutor = new DisabledWebApiEndpointMiddlewareExecutor<Command, Response>();

        var apiEndpoint = new ApiEndpoint(true);

        var httpContext = new DefaultHttpContext();

        var request = new Command();

        var result = await middlewareExecutor.ExecuteAsync(httpContext, request,
                                                           (c, ct) => apiEndpoint.ExecuteCommandAsync(c, ct),
                                                           CancellationToken.None);
        
        result.ShouldBeSuccessWithValue(new Response());
        
        apiEndpoint.WasCalled.Should().BeTrue();
    }

    [Fact]
    public async Task verify_calls_ApiEndpoint_and_returns_correctly_for_failure()
    {
        var middlewareExecutor = new DisabledWebApiEndpointMiddlewareExecutor<Command, Response>();

        var apiEndpoint = new ApiEndpoint(false);

        var httpContext = new DefaultHttpContext();

        var request = new Command();

        var result = await middlewareExecutor.ExecuteAsync(httpContext, request,
                                                           (c, ct) => apiEndpoint.ExecuteCommandAsync(c, ct),
                                                           CancellationToken.None);
        
        result.ShouldBeFailureWithError(ApiEndpoint.ErrorMessage);
        
        apiEndpoint.WasCalled.Should().BeTrue();
    }
}