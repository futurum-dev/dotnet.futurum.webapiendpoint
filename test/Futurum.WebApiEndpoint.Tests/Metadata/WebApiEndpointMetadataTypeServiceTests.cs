using FluentAssertions;

using Futurum.Core.Functional;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Internal.Dispatcher;
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

    public class QueryWithoutRequest
    {
        [Fact]
        public void check()
        {
            var apiEndpointInterfaceType = typeof(IQueryWebApiEndpoint<ResponseDto, Response, Mapper>);
            var apiEndpointType = typeof(ApiEndpoint);

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForQueryWithoutRequest(apiEndpointInterfaceType, apiEndpointType);

            metadataTypeDefinition.RequestDtoType.Should().Be<EmptyRequestDto>();
            metadataTypeDefinition.ResponseDtoType.Should().Be<ResponseDto>();
            metadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Unit, Response>>();
            metadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<QueryWebApiEndpointDispatcher<ResponseDto, Response, Mapper>>();
            metadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<IQueryWebApiEndpoint<ResponseDto, Response, Mapper>>();
            metadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
            metadataTypeDefinition.MapperTypes.Should().BeEquivalentTo(new[] { typeof(Mapper) });
        }

        public record ResponseDto;

        public record Response;

        public class ApiEndpoint : QueryWebApiEndpoint.WithoutRequest.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
        {
            protected override Task<Result<Response>> ExecuteAsync(CancellationToken cancellationToken) =>
                new Response().ToResultOkAsync();
        }

        public class Mapper : IWebApiEndpointResponseMapper<Response, ResponseDto>
        {
            public ResponseDto Map(Response domain) =>
                throw new NotImplementedException();
        }
    }

    public class QueryWithoutRequestDto
    {
        [Fact]
        public void check()
        {
            var apiEndpointInterfaceType = typeof(IQueryWebApiEndpoint<ResponseDto, Request, Response, Mapper, Mapper>);
            var apiEndpointType = typeof(ApiEndpoint);

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForQueryWithoutRequestDto(apiEndpointInterfaceType, apiEndpointType);

            metadataTypeDefinition.RequestDtoType.Should().Be<EmptyRequestDto>();
            metadataTypeDefinition.ResponseDtoType.Should().Be<ResponseDto>();
            metadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Request, Response>>();
            metadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<QueryWebApiEndpointDispatcher<ResponseDto, Request, Response, Mapper, Mapper>>();
            metadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<IQueryWebApiEndpoint<ResponseDto, Request, Response, Mapper, Mapper>>();
            metadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
            metadataTypeDefinition.MapperTypes.Should().BeEquivalentTo(new[] { typeof(Mapper), typeof(Mapper) });
        }

        public record Request;

        public record ResponseDto;

        public record Response;

        public class ApiEndpoint : QueryWebApiEndpoint.WithRequest<Request>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
        {
            protected override Task<Result<Response>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
                new Response().ToResultOkAsync();
        }

        public class Mapper : IWebApiEndpointResponseMapper<Response, ResponseDto>, IWebApiEndpointRequestMapper<Request>
        {
            public ResponseDto Map(Response domain) =>
                throw new NotImplementedException();

            public Result<Request> Map(HttpContext httpContext) =>
                throw new NotImplementedException();
        }
    }

    public class QueryWithRequestDto
    {
        [Fact]
        public void check()
        {
            var apiEndpointInterfaceType = typeof(IQueryWebApiEndpoint<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>);
            var apiEndpointType = typeof(ApiEndpoint);

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForQueryWithRequestDto(apiEndpointInterfaceType, apiEndpointType);

            metadataTypeDefinition.RequestDtoType.Should().Be<RequestDto>();
            metadataTypeDefinition.ResponseDtoType.Should().Be<ResponseDto>();
            metadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Request, Response>>();
            metadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<QueryWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>>();
            metadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<IQueryWebApiEndpoint<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>>();
            metadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
            metadataTypeDefinition.MapperTypes.Should().BeEquivalentTo(new[] { typeof(Mapper), typeof(Mapper) });
        }

        public record RequestDto;

        public record Request;

        public record ResponseDto;

        public record Response;

        public class ApiEndpoint : QueryWebApiEndpoint.WithRequest<RequestDto, Request>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
        {
            protected override Task<Result<Response>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
                new Response().ToResultOkAsync();
        }

        public class Mapper : IWebApiEndpointResponseMapper<Response, ResponseDto>, IWebApiEndpointRequestMapper<RequestDto, Request>
        {
            public ResponseDto Map(Response domain) =>
                throw new NotImplementedException();

            public Result<Request> Map(HttpContext httpContext, RequestDto dto) =>
                throw new NotImplementedException();
        }
    }

    public class CommandWithRequestWithResponse
    {
        [Fact]
        public void check()
        {
            var apiEndpointInterfaceType = typeof(ICommandWebApiEndpoint<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>);
            var apiEndpointType = typeof(ApiEndpoint);

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForCommandWithRequestWithResponse(apiEndpointInterfaceType, apiEndpointType);

            metadataTypeDefinition.RequestDtoType.Should().Be<CommandDto>();
            metadataTypeDefinition.ResponseDtoType.Should().Be<ResponseDto>();
            metadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Command, Response>>();
            metadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<CommandWebApiEndpointDispatcher<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>>();
            metadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<ICommandWebApiEndpoint<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>>();
            metadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
            metadataTypeDefinition.MapperTypes.Should().BeEquivalentTo(new[] { typeof(Mapper), typeof(Mapper) });
        }

        public record CommandDto;

        public record Command;

        public record ResponseDto;

        public record Response;

        public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
        {
            protected override Task<Result<Response>> ExecuteAsync(Command query, CancellationToken cancellationToken) =>
                new Response().ToResultOkAsync();
        }

        public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>, IWebApiEndpointResponseMapper<Response, ResponseDto>
        {
            public Result<Command> Map(HttpContext httpContext, CommandDto dto) =>
                throw new NotImplementedException();

            public ResponseDto Map(Response domain) =>
                throw new NotImplementedException();
        }
    }

    public class CommandWithoutRequestWithResponse
    {
        [Fact]
        public void check()
        {
            var apiEndpointInterfaceType = typeof(ICommandWebApiEndpoint<ResponseDto, Command, Response, Mapper, Mapper>);
            var apiEndpointType = typeof(ApiEndpoint);

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForCommandWithoutRequestWithResponse(apiEndpointInterfaceType, apiEndpointType);

            metadataTypeDefinition.RequestDtoType.Should().Be<EmptyRequestDto>();
            metadataTypeDefinition.ResponseDtoType.Should().Be<ResponseDto>();
            metadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Command, Response>>();
            metadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<CommandWebApiEndpointDispatcher<ResponseDto, Command, Response, Mapper, Mapper>>();
            metadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<ICommandWebApiEndpoint<ResponseDto, Command, Response, Mapper, Mapper>>();
            metadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
            metadataTypeDefinition.MapperTypes.Should().BeEquivalentTo(new[] { typeof(Mapper), typeof(Mapper) });
        }

        public record Command;

        public record ResponseDto;

        public record Response;

        public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<Command>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
        {
            protected override Task<Result<Response>> ExecuteAsync(Command query, CancellationToken cancellationToken) =>
                new Response().ToResultOkAsync();
        }

        public class Mapper : IWebApiEndpointRequestMapper<Command>, IWebApiEndpointResponseMapper<Response, ResponseDto>
        {
            public Result<Command> Map(HttpContext httpContext) =>
                throw new NotImplementedException();

            public ResponseDto Map(Response domain) =>
                throw new NotImplementedException();
        }
    }

    public class CommandWithoutResponse
    {
        [Fact]
        public void check()
        {
            var apiEndpointInterfaceType = typeof(ICommandWebApiEndpoint<CommandDto, Command, Mapper>);
            var apiEndpointType = typeof(ApiEndpoint);

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForCommandWithoutResponse(apiEndpointInterfaceType, apiEndpointType);

            metadataTypeDefinition.RequestDtoType.Should().Be<CommandDto>();
            metadataTypeDefinition.ResponseDtoType.Should().Be<EmptyResponseDto>();
            metadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Command, Unit>>();
            metadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<CommandWebApiEndpointDispatcher<CommandDto, Command, Mapper>>();
            metadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<ICommandWebApiEndpoint<CommandDto, Command, Mapper>>();
            metadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
            metadataTypeDefinition.MapperTypes.Should().BeEquivalentTo(new[] { typeof(Mapper) });
        }

        public record CommandDto;

        public record Command;

        public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithoutResponse.WithMapper<Mapper>
        {
            protected override Task<Result> ExecuteAsync(Command query, CancellationToken cancellationToken) =>
                Result.OkAsync();
        }

        public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
        {
            public Result<Command> Map(HttpContext httpContext, CommandDto dto) =>
                throw new NotImplementedException();
        }
    }

    public class CommandWithoutRequestWithoutResponse
    {
        [Fact]
        public void check()
        {
            var apiEndpointInterfaceType = typeof(ICommandWebApiEndpoint<Command, Mapper>);
            var apiEndpointType = typeof(ApiEndpoint);

            var metadataTypeDefinition = WebApiEndpointMetadataTypeService.GetForCommandWithoutRequestWithoutResponse(apiEndpointInterfaceType, apiEndpointType);

            metadataTypeDefinition.RequestDtoType.Should().Be<EmptyRequestDto>();
            metadataTypeDefinition.ResponseDtoType.Should().Be<EmptyResponseDto>();
            metadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Command, Unit>>();
            metadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<CommandWebApiEndpointDispatcher<Command, Mapper>>();
            metadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<ICommandWebApiEndpoint<Command, Mapper>>();
            metadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
            metadataTypeDefinition.MapperTypes.Should().BeEquivalentTo(new[] { typeof(Mapper) });
        }

        public record Command;

        public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<Command>.WithoutResponse.WithMapper<Mapper>
        {
            protected override Task<Result> ExecuteAsync(Command query, CancellationToken cancellationToken) =>
                Result.OkAsync();
        }

        public class Mapper : IWebApiEndpointRequestMapper<Command>
        {
            public Result<Command> Map(HttpContext httpContext) =>
                throw new NotImplementedException();
        }
    }
}