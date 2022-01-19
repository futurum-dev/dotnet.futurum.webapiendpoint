using FluentAssertions;

using Futurum.ApiEndpoint;
using Futurum.Core.Functional;
using Futurum.Core.Linq;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Internal.Dispatcher;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

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

            public class ApiEndpoint : Futurum.WebApiEndpoint.QueryWebApiEndpoint.WithoutRequest.WithResponse<ResponseDto, Response>
            {
                protected override Task<Result<Response>> ExecuteAsync(CancellationToken cancellationToken) =>
                    new Response().ToResultOkAsync();
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
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<QueryWebApiEndpointDispatcher<ResponseDto, Response>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<IQueryWebApiEndpoint<ResponseDto, Response>>();
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

            public class ApiEndpoint : Futurum.WebApiEndpoint.QueryWebApiEndpoint.WithRequest<Request>.WithResponse<ResponseDto, Response>
            {
                protected override Task<Result<Response>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
                    new Response().ToResultOkAsync();
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
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<QueryWebApiEndpointDispatcher<ResponseDto, Request, Response>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<IQueryWebApiEndpoint<ResponseDto, Request, Response>>();
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

            public class ApiEndpoint : Futurum.WebApiEndpoint.QueryWebApiEndpoint.WithRequest<RequestDto, Request>.WithResponse<ResponseDto, Response>
            {
                protected override Task<Result<Response>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
                    new Response().ToResultOkAsync();
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
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<QueryWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<IQueryWebApiEndpoint<RequestDto, ResponseDto, Request, Response>>();
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

            public class ApiEndpoint : Futurum.WebApiEndpoint.CommandWebApiEndpoint.WithRequest<RequestDto, Request>.WithResponse<ResponseDto, Response>
            {
                protected override Task<Result<Response>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
                    new Response().ToResultOkAsync();
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
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<CommandWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<ICommandWebApiEndpoint<RequestDto, ResponseDto, Request, Response>>();
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

            public class ApiEndpoint : Futurum.WebApiEndpoint.CommandWebApiEndpoint.WithRequest<Request>.WithResponse<ResponseDto, Response>
            {
                protected override Task<Result<Response>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
                    new Response().ToResultOkAsync();
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
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<CommandWebApiEndpointDispatcher<ResponseDto, Request, Response>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<ICommandWebApiEndpoint<ResponseDto, Request, Response>>();
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

            public class ApiEndpoint : Futurum.WebApiEndpoint.CommandWebApiEndpoint.WithRequest<RequestDto, Request>.WithoutResponse
            {
                protected override Task<Result> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
                    Result.OkAsync();
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
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<CommandWebApiEndpointDispatcher<RequestDto, Request>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<ICommandWebApiEndpoint<RequestDto, Request>>();
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

            public class ApiEndpoint : Futurum.WebApiEndpoint.CommandWebApiEndpoint.WithRequest<Request>.WithoutResponse
            {
                protected override Task<Result> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
                    Result.OkAsync();
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
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<CommandWebApiEndpointDispatcher<Request>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<ICommandWebApiEndpoint<Request>>();
                metadataDefinition.MetadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
                metadataDefinition.MetadataMapFromDefinition.Should().BeNull();
            }
        }
    }
}