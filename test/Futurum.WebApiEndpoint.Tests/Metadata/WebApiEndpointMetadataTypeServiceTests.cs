using FluentAssertions;

using Futurum.Core.Functional;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Internal.Dispatcher;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

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

    public class QueryWithoutRequest
    {
        [Fact]
        public void check()
        {
            var apiEndpointInterfaceType = typeof(IQueryWebApiEndpoint<ResponseDto, Response>);
            var apiEndpointType = typeof(ApiEndpoint);

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForQueryWithoutRequest(apiEndpointInterfaceType, apiEndpointType);

            metadataTypeDefinition.RequestDtoType.Should().Be<EmptyRequestDto>();
            metadataTypeDefinition.ResponseDtoType.Should().Be<ResponseDto>();
            metadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Unit, Response>>();
            metadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<QueryWebApiEndpointDispatcher<ResponseDto, Response>>();
            metadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<IQueryWebApiEndpoint<ResponseDto, Response>>();
            metadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
        }

        public record ResponseDto;

        public record Response;

        public class ApiEndpoint : QueryWebApiEndpoint.WithoutRequest.WithResponse<ResponseDto, Response>
        {
            protected override Task<Result<Response>> ExecuteAsync(CancellationToken cancellationToken) =>
                new Response().ToResultOkAsync();
        }
    }

    public class QueryWithoutRequestDto
    {
        [Fact]
        public void check()
        {
            var apiEndpointInterfaceType = typeof(IQueryWebApiEndpoint<ResponseDto, Request, Response>);
            var apiEndpointType = typeof(ApiEndpoint);

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForQueryWithoutRequestDto(apiEndpointInterfaceType, apiEndpointType);

            metadataTypeDefinition.RequestDtoType.Should().Be<EmptyRequestDto>();
            metadataTypeDefinition.ResponseDtoType.Should().Be<ResponseDto>();
            metadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Request, Response>>();
            metadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<QueryWebApiEndpointDispatcher<ResponseDto, Request, Response>>();
            metadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<IQueryWebApiEndpoint<ResponseDto, Request, Response>>();
            metadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
        }

        public record Request;

        public record ResponseDto;

        public record Response;

        public class ApiEndpoint : QueryWebApiEndpoint.WithRequest<Request>.WithResponse<ResponseDto, Response>
        {
            protected override Task<Result<Response>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
                new Response().ToResultOkAsync();
        }
    }

    public class QueryWithRequestDto
    {
        [Fact]
        public void check()
        {
            var apiEndpointInterfaceType = typeof(IQueryWebApiEndpoint<RequestDto, ResponseDto, Request, Response>);
            var apiEndpointType = typeof(ApiEndpoint);

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForQueryWithRequestDto(apiEndpointInterfaceType, apiEndpointType);

            metadataTypeDefinition.RequestDtoType.Should().Be<RequestDto>();
            metadataTypeDefinition.ResponseDtoType.Should().Be<ResponseDto>();
            metadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Request, Response>>();
            metadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<QueryWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response>>();
            metadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<IQueryWebApiEndpoint<RequestDto, ResponseDto, Request, Response>>();
            metadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
        }

        public record RequestDto;

        public record Request;

        public record ResponseDto;

        public record Response;

        public class ApiEndpoint : QueryWebApiEndpoint.WithRequest<RequestDto, Request>.WithResponse<ResponseDto, Response>
        {
            protected override Task<Result<Response>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
                new Response().ToResultOkAsync();
        }
    }

    public class CommandWithRequestWithResponse
    {
        [Fact]
        public void check()
        {
            var apiEndpointInterfaceType = typeof(ICommandWebApiEndpoint<CommandDto, ResponseDto, Command, Response>);
            var apiEndpointType = typeof(ApiEndpoint);

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForCommandWithRequestWithResponse(apiEndpointInterfaceType, apiEndpointType);

            metadataTypeDefinition.RequestDtoType.Should().Be<CommandDto>();
            metadataTypeDefinition.ResponseDtoType.Should().Be<ResponseDto>();
            metadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Command, Response>>();
            metadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<CommandWebApiEndpointDispatcher<CommandDto, ResponseDto, Command, Response>>();
            metadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<ICommandWebApiEndpoint<CommandDto, ResponseDto, Command, Response>>();
            metadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
        }

        public record CommandDto;

        public record Command;

        public record ResponseDto;

        public record Response;

        public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponse<ResponseDto, Response>
        {
            protected override Task<Result<Response>> ExecuteAsync(Command query, CancellationToken cancellationToken) =>
                new Response().ToResultOkAsync();
        }
    }

    public class CommandWithoutRequestWithResponse
    {
        [Fact]
        public void check()
        {
            var apiEndpointInterfaceType = typeof(ICommandWebApiEndpoint<ResponseDto, Command, Response>);
            var apiEndpointType = typeof(ApiEndpoint);

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForCommandWithoutRequestWithResponse(apiEndpointInterfaceType, apiEndpointType);

            metadataTypeDefinition.RequestDtoType.Should().Be<EmptyRequestDto>();
            metadataTypeDefinition.ResponseDtoType.Should().Be<ResponseDto>();
            metadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Command, Response>>();
            metadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<CommandWebApiEndpointDispatcher<ResponseDto, Command, Response>>();
            metadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<ICommandWebApiEndpoint<ResponseDto, Command, Response>>();
            metadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
        }

        public record Command;

        public record ResponseDto;

        public record Response;

        public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<Command>.WithResponse<ResponseDto, Response>
        {
            protected override Task<Result<Response>> ExecuteAsync(Command query, CancellationToken cancellationToken) =>
                new Response().ToResultOkAsync();
        }
    }

    public class CommandWithoutResponse
    {
        [Fact]
        public void check()
        {
            var apiEndpointInterfaceType = typeof(ICommandWebApiEndpoint<CommandDto, Command>);
            var apiEndpointType = typeof(ApiEndpoint);

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForCommandWithoutResponse(apiEndpointInterfaceType, apiEndpointType);

            metadataTypeDefinition.RequestDtoType.Should().Be<CommandDto>();
            metadataTypeDefinition.ResponseDtoType.Should().Be<EmptyResponseDto>();
            metadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Command, Unit>>();
            metadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<CommandWebApiEndpointDispatcher<CommandDto, Command>>();
            metadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<ICommandWebApiEndpoint<CommandDto, Command>>();
            metadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
        }

        public record CommandDto;

        public record Command;

        public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithoutResponse
        {
            protected override Task<Result> ExecuteAsync(Command query, CancellationToken cancellationToken) =>
                Result.OkAsync();
        }
    }

    public class CommandWithoutRequestWithoutResponse
    {
        [Fact]
        public void check()
        {
            var apiEndpointInterfaceType = typeof(ICommandWebApiEndpoint<Command>);
            var apiEndpointType = typeof(ApiEndpoint);

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForCommandWithoutRequestWithoutResponse(apiEndpointInterfaceType, apiEndpointType);

            metadataTypeDefinition.RequestDtoType.Should().Be<EmptyRequestDto>();
            metadataTypeDefinition.ResponseDtoType.Should().Be<EmptyResponseDto>();
            metadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Command, Unit>>();
            metadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<CommandWebApiEndpointDispatcher<Command>>();
            metadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<ICommandWebApiEndpoint<Command>>();
            metadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
        }

        public record Command;

        public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<Command>.WithoutResponse
        {
            protected override Task<Result> ExecuteAsync(Command query, CancellationToken cancellationToken) =>
                Result.OkAsync();
        }
    }
}