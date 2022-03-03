using FluentAssertions;

using Futurum.Core.Result;
using Futurum.Test.Result;
using Futurum.WebApiEndpoint.Metadata;
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
    
    private class ApiEndpoint : CommandWebApiEndpoint.Request<CommandDto, Command>.Response<ResponseDto, Response>.Mapper<Mapper>
    {
        private readonly bool _isSuccess;

        public const string ErrorMessage = "ERROR-MESSAGE";

        public ApiEndpoint(bool isSuccess)
        {
            _isSuccess = isSuccess;
        }

        public bool WasCalled { get; private set; }

        public override Task<Result<Response>> ExecuteAsync(Command request, CancellationToken cancellationToken)
        {
            WasCalled = true;

            return _isSuccess
                ? new Response().ToResultOkAsync()
                : Result.FailAsync<Response>(ErrorMessage);
        }
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>, IWebApiEndpointResponseDtoMapper<Response, ResponseDto>
    {
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CommandDto dto, CancellationToken cancellationToken) =>
            throw new NotImplementedException();

        public ResponseDto Map(Response domain) =>
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
                                                           (c, ct) => apiEndpoint.ExecuteAsync(c, ct),
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
                                                           (c, ct) => apiEndpoint.ExecuteAsync(c, ct),
                                                           CancellationToken.None);
        
        result.ShouldBeFailureWithError(ApiEndpoint.ErrorMessage);
        
        apiEndpoint.WasCalled.Should().BeTrue();
    }
}