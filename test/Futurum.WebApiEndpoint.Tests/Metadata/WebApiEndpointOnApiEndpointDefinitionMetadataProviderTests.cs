using FluentAssertions;

using Futurum.ApiEndpoint;
using Futurum.Core.Linq;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
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

        public class ApiEndpoint : Futurum.WebApiEndpoint.QueryWebApiEndpoint.Request<RequestDto, Request>.Response<ResponseDto, Response>.Mapper<Mapper>
        {
            public override Task<Result<Response>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
                throw new NotImplementedException();
        }

        public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>, IWebApiEndpointResponseDtoMapper<Response, ResponseDto>
        {
            public Task<Result<Request>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, RequestDto dto, CancellationToken cancellationToken) =>
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
            metadataDefinition.MetadataTypeDefinition.RequestDtoType.Should().Be<RequestJsonDto<RequestDto>>();
            metadataDefinition.MetadataTypeDefinition.UnderlyingRequestDtoType.Should().Be<RequestDto>();
            metadataDefinition.MetadataTypeDefinition.ResponseDtoType.Should().Be<ResponseJsonDto<ResponseDto>>();
            metadataDefinition.MetadataTypeDefinition.UnderlyingResponseDtoType.Should().Be<ResponseDto>();
            metadataDefinition.MetadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Request, Response>>();
            metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<WebApiEndpointDispatcher<RequestJsonDto<RequestDto>, ResponseJsonDto<ResponseDto>, Request, Response, 
                RequestJsonMapper<RequestDto, Request, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>>();
            metadataDefinition.MetadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<IQueryWebApiEndpoint<RequestJsonDto<RequestDto>, ResponseJsonDto<ResponseDto>, Request, Response, 
                RequestJsonMapper<RequestDto, Request, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>>();
            metadataDefinition.MetadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
            metadataDefinition.MetadataMapFromDefinition.Should().BeNull();
        }
    }
    
    public class CommandWebApiEndpoint
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

        public class ApiEndpoint : Futurum.WebApiEndpoint.CommandWebApiEndpoint.Request<RequestDto, Request>.Response<ResponseDto, Response>.Mapper<Mapper>
        {
            public override Task<Result<Response>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
                new Response().ToResultOkAsync();
        }

        public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>, IWebApiEndpointResponseDtoMapper<Response, ResponseDto>
        {
            public Task<Result<Request>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, RequestDto dto, CancellationToken cancellationToken) =>
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
            metadataDefinition.MetadataTypeDefinition.RequestDtoType.Should().Be<RequestJsonDto<RequestDto>>();
            metadataDefinition.MetadataTypeDefinition.UnderlyingRequestDtoType.Should().Be<RequestDto>();
            metadataDefinition.MetadataTypeDefinition.ResponseDtoType.Should().Be<ResponseJsonDto<ResponseDto>>();
            metadataDefinition.MetadataTypeDefinition.UnderlyingResponseDtoType.Should().Be<ResponseDto>();
            metadataDefinition.MetadataTypeDefinition.MiddlewareExecutorType.Should().Be<IWebApiEndpointMiddlewareExecutor<Request, Response>>();
            metadataDefinition.MetadataTypeDefinition.WebApiEndpointExecutorServiceType.Should().Be<WebApiEndpointDispatcher<RequestJsonDto<RequestDto>, ResponseJsonDto<ResponseDto>, Request, Response, 
                RequestJsonMapper<RequestDto, Request, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>>();
            metadataDefinition.MetadataTypeDefinition.WebApiEndpointInterfaceType.Should().Be<ICommandWebApiEndpoint<RequestJsonDto<RequestDto>, ResponseJsonDto<ResponseDto>, Request, Response, 
                RequestJsonMapper<RequestDto, Request, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>>();
            metadataDefinition.MetadataTypeDefinition.WebApiEndpointType.Should().Be<ApiEndpoint>();
            metadataDefinition.MetadataMapFromDefinition.Should().BeNull();
        }
    }
}