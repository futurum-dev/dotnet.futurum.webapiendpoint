using FluentAssertions;

using Futurum.ApiEndpoint;
using Futurum.Core.Functional;
using Futurum.Core.Linq;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Internal.Dispatcher;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

using Microsoft.AspNetCore.Http;

using Xunit;
using Xunit.Abstractions;

namespace Futurum.WebApiEndpoint.Tests.Metadata;

public class WebApiEndpointOnApiEndpointDefinitionMetadataProviderTests
{
    private readonly ITestOutputHelper _output;

    public WebApiEndpointOnApiEndpointDefinitionMetadataProviderTests(ITestOutputHelper output)
    {
        _output = output;
    }

    public class QueryWebApiEndpoint
    {
        public class QueryWithoutRequest
        {
            private const string Route = "Route";

            public class ApiEndpointDefinition : IApiEndpointDefinition
            {
                public void Configure(ApiEndpointDefinitionBuilder definitionBuilder)
                {
                    definitionBuilder.Web()
                                     .Query<ApiEndpoint>(builder => builder.Route(Route));
                }
            }

            public record ResponseDto;

            public record Response;

            public class ApiEndpoint : Futurum.WebApiEndpoint.QueryWebApiEndpoint.WithoutRequest.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
            {
                protected override Task<Result<Response>> ExecuteAsync(CancellationToken cancellationToken) =>
                    new Response().ToResultOkAsync();
            }

            public class Mapper : IWebApiEndpointResponseMapper<Response, ResponseDto>
            {
                public ResponseDto Map(Response domain) =>
                    throw new NotImplementedException();
            }

            [Fact]
            public void check_MetadataDefinition()
            {
                var metadataDefinitions = WebApiEndpointOnApiEndpointDefinitionMetadataProvider.GetMetadata(EnumerableExtensions.Return(new ApiEndpointDefinition()));

                var metadataDefinition = metadataDefinitions.Single();

                metadataDefinition.MetadataRouteDefinition.RouteTemplate.Should().Be(Route);
                metadataDefinition.MetadataTypeDefinition.RequestDtoType.Should().Be<EmptyRequestDto>();
                metadataDefinition.MetadataTypeDefinition.ResponseDtoType.Should().Be<ResponseDto>();
                metadataDefinition.MetadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Unit, Response>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<QueryWebApiEndpointDispatcher<ResponseDto, Response, Mapper>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<IQueryWebApiEndpoint<ResponseDto, Response, Mapper>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
                metadataDefinition.MetadataMapFromDefinition.Should().BeNull();
            }
        }

        public class QueryWithoutRequestDto
        {
            private const string Route = "Route";

            public class ApiEndpointDefinition : IApiEndpointDefinition
            {
                public void Configure(ApiEndpointDefinitionBuilder definitionBuilder)
                {
                    definitionBuilder.Web()
                                     .Query<ApiEndpoint>(builder => builder.Route(Route));
                }
            }

            public record Request;

            public record ResponseDto;

            public record Response;

            public class ApiEndpoint : Futurum.WebApiEndpoint.QueryWebApiEndpoint.WithRequest<Request>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
            {
                protected override Task<Result<Response>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
                    new Response().ToResultOkAsync();
            }

            public class Mapper : IWebApiEndpointRequestMapper<Request>, IWebApiEndpointResponseMapper<Response, ResponseDto>
            {
                public Result<Request> Map(HttpContext httpContext) =>
                    throw new NotImplementedException();

                public ResponseDto Map(Response domain) =>
                    throw new NotImplementedException();
            }

            [Fact]
            public void check_MetadataDefinition()
            {
                var metadataDefinitions = WebApiEndpointOnApiEndpointDefinitionMetadataProvider.GetMetadata(EnumerableExtensions.Return(new ApiEndpointDefinition()));

                var metadataDefinition = metadataDefinitions.Single();

                metadataDefinition.MetadataRouteDefinition.RouteTemplate.Should().Be(Route);
                metadataDefinition.MetadataTypeDefinition.RequestDtoType.Should().Be<EmptyRequestDto>();
                metadataDefinition.MetadataTypeDefinition.ResponseDtoType.Should().Be<ResponseDto>();
                metadataDefinition.MetadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Request, Response>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<QueryWebApiEndpointDispatcher<ResponseDto, Request, Response, Mapper, Mapper>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<IQueryWebApiEndpoint<ResponseDto, Request, Response, Mapper, Mapper>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
                metadataDefinition.MetadataMapFromDefinition.Should().BeNull();
            }
        }

        public class QueryWithRequestDto
        {
            private const string Route = "Route";

            public class ApiEndpointDefinition : IApiEndpointDefinition
            {
                public void Configure(ApiEndpointDefinitionBuilder definitionBuilder)
                {
                    definitionBuilder.Web()
                                     .Query<ApiEndpoint>(builder => builder.Route(Route));
                }
            }

            public record RequestDto;

            public record Request;

            public record ResponseDto;

            public record Response;

            public class ApiEndpoint : Futurum.WebApiEndpoint.QueryWebApiEndpoint.WithRequest<RequestDto, Request>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
            {
                protected override Task<Result<Response>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
                    new Response().ToResultOkAsync();
            }

            public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>, IWebApiEndpointResponseMapper<Response, ResponseDto>
            {
                public Result<Request> Map(HttpContext httpContext, RequestDto dto) =>
                    throw new NotImplementedException();

                public ResponseDto Map(Response domain) =>
                    throw new NotImplementedException();
            }

            [Fact]
            public void check_MetadataDefinition()
            {
                var metadataDefinitions = WebApiEndpointOnApiEndpointDefinitionMetadataProvider.GetMetadata(EnumerableExtensions.Return(new ApiEndpointDefinition()));

                var metadataDefinition = metadataDefinitions.Single();

                metadataDefinition.MetadataRouteDefinition.RouteTemplate.Should().Be(Route);
                metadataDefinition.MetadataTypeDefinition.RequestDtoType.Should().Be<RequestDto>();
                metadataDefinition.MetadataTypeDefinition.ResponseDtoType.Should().Be<ResponseDto>();
                metadataDefinition.MetadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Request, Response>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<QueryWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<IQueryWebApiEndpoint<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
                metadataDefinition.MetadataMapFromDefinition.Should().BeNull();
            }
        }
    }

    public class CommandWebApiEndpoint
    {
        public class CommandWithRequestWithResponse
        {
            private const string Route = "Route";

            public class ApiEndpointDefinition : IApiEndpointDefinition
            {
                public void Configure(ApiEndpointDefinitionBuilder definitionBuilder)
                {
                    definitionBuilder.Web()
                                     .Command<ApiEndpoint>(builder => builder.Post(Route));
                }
            }

            public record RequestDto;

            public record Request;

            public record ResponseDto;

            public record Response;

            public class ApiEndpoint : Futurum.WebApiEndpoint.CommandWebApiEndpoint.WithRequest<RequestDto, Request>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
            {
                protected override Task<Result<Response>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
                    new Response().ToResultOkAsync();
            }

            public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>, IWebApiEndpointResponseMapper<Response, ResponseDto>
            {
                public Result<Request> Map(HttpContext httpContext, RequestDto dto) =>
                    throw new NotImplementedException();

                public ResponseDto Map(Response domain) =>
                    throw new NotImplementedException();
            }

            [Fact]
            public void check_MetadataDefinition()
            {
                var metadataDefinitions = WebApiEndpointOnApiEndpointDefinitionMetadataProvider.GetMetadata(EnumerableExtensions.Return(new ApiEndpointDefinition()));

                var metadataDefinition = metadataDefinitions.Single();

                metadataDefinition.MetadataRouteDefinition.RouteTemplate.Should().Be(Route);
                metadataDefinition.MetadataTypeDefinition.RequestDtoType.Should().Be<RequestDto>();
                metadataDefinition.MetadataTypeDefinition.ResponseDtoType.Should().Be<ResponseDto>();
                metadataDefinition.MetadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Request, Response>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<CommandWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<ICommandWebApiEndpoint<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
                metadataDefinition.MetadataMapFromDefinition.Should().BeNull();
            }
        }

        public class CommandWithoutRequestWithResponse
        {
            private const string Route = "Route";

            public class ApiEndpointDefinition : IApiEndpointDefinition
            {
                public void Configure(ApiEndpointDefinitionBuilder definitionBuilder)
                {
                    definitionBuilder.Web()
                                     .Command<ApiEndpoint>(builder => builder.Post(Route));
                }
            }

            public record Request;

            public record ResponseDto;

            public record Response;

            public class ApiEndpoint : Futurum.WebApiEndpoint.CommandWebApiEndpoint.WithRequest<Request>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
            {
                protected override Task<Result<Response>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
                    new Response().ToResultOkAsync();
            }

            public class Mapper : IWebApiEndpointRequestMapper<Request>, IWebApiEndpointResponseMapper<Response, ResponseDto>
            {
                public Result<Request> Map(HttpContext httpContext) =>
                    throw new NotImplementedException();

                public ResponseDto Map(Response domain) =>
                    throw new NotImplementedException();
            }

            [Fact]
            public void check_MetadataDefinition()
            {
                var metadataDefinitions = WebApiEndpointOnApiEndpointDefinitionMetadataProvider.GetMetadata(EnumerableExtensions.Return(new ApiEndpointDefinition()));

                var metadataDefinition = metadataDefinitions.Single();

                metadataDefinition.MetadataRouteDefinition.RouteTemplate.Should().Be(Route);
                metadataDefinition.MetadataTypeDefinition.RequestDtoType.Should().Be<EmptyRequestDto>();
                metadataDefinition.MetadataTypeDefinition.ResponseDtoType.Should().Be<ResponseDto>();
                metadataDefinition.MetadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Request, Response>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<CommandWebApiEndpointDispatcher<ResponseDto, Request, Response, Mapper, Mapper>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<ICommandWebApiEndpoint<ResponseDto, Request, Response, Mapper, Mapper>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
                metadataDefinition.MetadataMapFromDefinition.Should().BeNull();
            }
        }

        public class CommandWithResponse
        {
            private const string Route = "Route";

            public class ApiEndpointDefinition : IApiEndpointDefinition
            {
                public void Configure(ApiEndpointDefinitionBuilder definitionBuilder)
                {
                    definitionBuilder.Web()
                                     .Command<ApiEndpoint>(builder => builder.Post(Route));
                }
            }

            public record RequestDto;

            public record Request;

            public class ApiEndpoint : Futurum.WebApiEndpoint.CommandWebApiEndpoint.WithRequest<RequestDto, Request>.WithoutResponse.WithMapper<Mapper>
            {
                protected override Task<Result> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
                    Result.OkAsync();
            }

            public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>
            {
                public Result<Request> Map(HttpContext httpContext, RequestDto dto) =>
                    throw new NotImplementedException();
            }

            [Fact]
            public void check_MetadataDefinition()
            {
                var metadataDefinitions = WebApiEndpointOnApiEndpointDefinitionMetadataProvider.GetMetadata(EnumerableExtensions.Return(new ApiEndpointDefinition()));

                var metadataDefinition = metadataDefinitions.Single();

                metadataDefinition.MetadataRouteDefinition.RouteTemplate.Should().Be(Route);
                metadataDefinition.MetadataTypeDefinition.RequestDtoType.Should().Be<RequestDto>();
                metadataDefinition.MetadataTypeDefinition.ResponseDtoType.Should().Be<EmptyResponseDto>();
                metadataDefinition.MetadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Request, Unit>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<CommandWebApiEndpointDispatcher<RequestDto, Request, Mapper>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<ICommandWebApiEndpoint<RequestDto, Request, Mapper>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
                metadataDefinition.MetadataMapFromDefinition.Should().BeNull();
            }
        }

        public class CommandWithoutRequestWithoutResponse
        {
            private const string Route = "Route";

            public class ApiEndpointDefinition : IApiEndpointDefinition
            {
                public void Configure(ApiEndpointDefinitionBuilder definitionBuilder)
                {
                    definitionBuilder.Web()
                                     .Command<ApiEndpoint>(builder => builder.Post(Route));
                }
            }

            public record Request;

            public class ApiEndpoint : Futurum.WebApiEndpoint.CommandWebApiEndpoint.WithRequest<Request>.WithoutResponse.WithMapper<Mapper>
            {
                protected override Task<Result> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
                    Result.OkAsync();
            }

            public class Mapper : IWebApiEndpointRequestMapper<Request>
            {
                public Result<Request> Map(HttpContext httpContext) =>
                    throw new NotImplementedException();
            }

            [Fact]
            public void check_MetadataDefinition()
            {
                var metadataDefinitions = WebApiEndpointOnApiEndpointDefinitionMetadataProvider.GetMetadata(EnumerableExtensions.Return(new ApiEndpointDefinition()));

                var metadataDefinition = metadataDefinitions.Single();

                metadataDefinition.MetadataRouteDefinition.RouteTemplate.Should().Be(Route);
                metadataDefinition.MetadataTypeDefinition.RequestDtoType.Should().Be<EmptyRequestDto>();
                metadataDefinition.MetadataTypeDefinition.ResponseDtoType.Should().Be<EmptyResponseDto>();
                metadataDefinition.MetadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Request, Unit>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<CommandWebApiEndpointDispatcher<Request, Mapper>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<ICommandWebApiEndpoint<Request, Mapper>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
                metadataDefinition.MetadataMapFromDefinition.Should().BeNull();
            }
        }
    }
}