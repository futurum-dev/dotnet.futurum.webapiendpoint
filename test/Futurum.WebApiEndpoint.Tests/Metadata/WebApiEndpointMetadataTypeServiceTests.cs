using FluentAssertions;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

using Microsoft.AspNetCore.Http;

using Xunit;
using Xunit.Abstractions;

namespace Futurum.WebApiEndpoint.Tests.Metadata;

public class WebApiEndpointMetadataTypeServiceTests
{
    private readonly ITestOutputHelper _output;

    public WebApiEndpointMetadataTypeServiceTests(ITestOutputHelper output)
    {
        _output = output;
    }


    [Fact]
    public void check()
    {
        var apiEndpointInterfaceType = typeof(ICommandWebApiEndpoint<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>);
        var apiEndpointType = typeof(ApiEndpoint);

        var metadataTypeDefinition = WebApiEndpointMetadataTypeService.Get(apiEndpointInterfaceType, apiEndpointType);

        metadataTypeDefinition.RequestDtoType.Should().Be<CommandDto>();
        metadataTypeDefinition.ResponseDtoType.Should().Be<ResponseDto>();
        metadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Command, Response>>();
        metadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<WebApiEndpointDispatcher<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>>();
        metadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<ICommandWebApiEndpoint<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>>();
        metadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
        metadataTypeDefinition.MapperTypes.Should().BeEquivalentTo(new[] { typeof(Mapper), typeof(Mapper) });
    }

    public record CommandDto;

    public record Command;

    public record ResponseDto;

    public record Response;

    public class ApiEndpoint : CommandWebApiEndpoint.Request<CommandDto, Command>.Response<ResponseDto, Response>.Mapper<Mapper>
    {
        public override Task<Result<Response>> ExecuteAsync(Command query, CancellationToken cancellationToken) =>
            new Response().ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<Command>, IWebApiEndpointResponseMapper<Response, ResponseDto>, IWebApiEndpointRequestMapper<CommandDto, Command>, IWebApiEndpointResponseDtoMapper<Response, ResponseDto>
    {
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
            throw new NotImplementedException();

        public Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, Response domain, CancellationToken cancellation) =>
            throw new NotImplementedException();

        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CommandDto dto, CancellationToken cancellationToken) =>
            throw new NotImplementedException();

        public ResponseDto Map(Response domain) =>
            throw new NotImplementedException();
    }
}