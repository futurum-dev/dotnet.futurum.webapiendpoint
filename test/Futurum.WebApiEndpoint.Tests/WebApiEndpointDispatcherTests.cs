using System.Text.Json;

using FluentAssertions;

using FluentValidation;

using Futurum.Core.Linq;
using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.Test.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

using Moq;
using Moq.AutoMock;

using Xunit;

using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Futurum.WebApiEndpoint.Tests;

public class WebApiEndpointDispatcherTests
{
    public class Validator : AbstractValidator<RequestDto>
    {
    }

    public record RequestDto;

    public record Request;

    public record ResponseDto;

    public record Response;

    private static readonly MetadataRouteDefinition MetadataRouteDefinition =
        new(MetadataRouteHttpMethod.Post, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, Option<Action<RouteHandlerBuilder>>.None, null);

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

        var mockWebApiEndpointLogger = new Mock<IWebApiEndpointLogger>();
        
        var result = await TestRunner(httpContext, apiEndpoint, autoMocker => autoMocker.Use<IWebApiEndpointLogger>(mockWebApiEndpointLogger.Object));

        result.ShouldBeSuccess();

        apiEndpoint.WasCalled.Should().BeTrue();

        httpContext.Response.StatusCode.Should().Be(MetadataRouteDefinition.FailedStatusCode);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var streamReader = new StreamReader(httpContext.Response.Body);
        var requestBody = await streamReader.ReadToEndAsync();

        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        problemDetails.Title.Should().Be(ReasonPhrases.GetReasonPhrase(MetadataRouteDefinition.FailedStatusCode));
        problemDetails.Detail.Should().Be(ApiEndpoint.ErrorMessage);
        
        mockWebApiEndpointLogger.Verify(x => x.Error(MetadataRouteDefinition.RouteTemplate, It.IsAny<IResultError>()), Times.Once);
    }

    private static async Task<Result> TestRunner(HttpContext httpContext, ApiEndpoint apiEndpoint, Action<AutoMocker>? autoMockerConfig = null)
    {
        var mocker = new AutoMocker();
        mocker.Use<IWebApiEndpointHttpContextDispatcher>(new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions())));
        mocker.Use<IWebApiEndpointRequestValidation<RequestDto>>(new WebApiEndpointRequestValidation<RequestDto>(EnumerableExtensions.Return(new Validator())));
        mocker.Use<IWebApiEndpointRequestMapper<RequestDto, Request>>(new Mapper());
        mocker.Use<IWebApiEndpointResponseDtoMapper<Response, ResponseDto>>(new Mapper());
        mocker.Use(new RequestJsonMapper<RequestDto, Request, Mapper>(new RequestJsonReader<RequestDto>(Options.Create(new JsonOptions())), new Mapper(), new WebApiEndpointRequestValidation<RequestDto>(EnumerableExtensions.Return(new Validator())) ));
        mocker.Use(new ResponseJsonMapper<Response, ResponseDto, Mapper>(Options.Create(new JsonOptions()), new Mapper()));
        autoMockerConfig?.Invoke(mocker);

        var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestJsonDto<RequestDto>),typeof(RequestDto), typeof(ResponseJsonDto<ResponseDto>),typeof(ResponseDto), typeof(ApiEndpoint),
                                                                typeof(ICommandWebApiEndpoint<RequestJsonDto<RequestDto>, ResponseJsonDto<ResponseDto>, Request, Response,
                                                                    RequestJsonMapper<RequestDto, Request, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>),
                                                                typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                typeof(WebApiEndpointDispatcher<RequestJsonDto<RequestDto>, ResponseJsonDto<ResponseDto>, Request, Response,
                                                                    RequestJsonMapper<RequestDto, Request, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>),
                                                                new List<Type>());
        var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
        var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());
        var metadataDefinition = new MetadataDefinition(MetadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

        var middlewareExecutor = new DisabledWebApiEndpointMiddlewareExecutor<Request, Response>();

        var commandWebApiEndpointDispatcher = mocker.CreateInstance<WebApiEndpointDispatcher<RequestJsonDto<RequestDto>, ResponseJsonDto<ResponseDto>, Request, Response,
            RequestJsonMapper<RequestDto, Request, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>>();

        return await commandWebApiEndpointDispatcher.ExecuteAsync(metadataDefinition, httpContext, middlewareExecutor, apiEndpoint, CancellationToken.None);
    }

    private class ApiEndpoint : CommandWebApiEndpoint.Request<RequestDto, Request>.Response<ResponseDto, Response>.Mapper<Mapper>
    {
        private readonly bool _isSuccess;

        public const string ErrorMessage = "ERROR-MESSAGE";

        public ApiEndpoint(bool isSuccess)
        {
            _isSuccess = isSuccess;
        }

        public bool WasCalled { get; private set; }

        public override Task<Result<Response>> ExecuteAsync(Request request, CancellationToken cancellationToken)
        {
            WasCalled = true;

            return _isSuccess
                ? new Response().ToResultOkAsync()
                : Result.FailAsync<Response>(ErrorMessage);
        }
    }

    public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>, IWebApiEndpointResponseDtoMapper<Response, ResponseDto>
    {
        public Task<Result<Request>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, RequestDto dto, CancellationToken cancellationToken) =>
            new Request().ToResultOkAsync();

        public ResponseDto Map(Response domain) =>
            new ();
    }
}