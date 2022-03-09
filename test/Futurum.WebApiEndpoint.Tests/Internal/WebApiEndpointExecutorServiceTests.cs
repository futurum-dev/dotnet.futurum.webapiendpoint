using System.Net;
using System.Text.Json;

using FluentAssertions;

using FluentValidation;

using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Moq;

using Xunit;
using Xunit.Abstractions;

using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Futurum.WebApiEndpoint.Tests.Internal;

public class WebApiEndpointExecutorServiceTests
{
    private readonly ITestOutputHelper _output;

    public WebApiEndpointExecutorServiceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    public record CommandDto;

    public record Command;

    public record ResponseDto(string FirstName, int Age);

    public record Response(string FirstName, int Age);

    private class SuccessApiEndpoint : CommandWebApiEndpoint.Request<CommandDto, Command>.Response<ResponseDto, Response>.Mapper<Mapper>
    {
        public override Task<Result<Response>> ExecuteAsync(Command request, CancellationToken cancellationToken) =>
            new Response("FirstName", 10).ToResultOkAsync();
    }

    private class FailedApiEndpoint : CommandWebApiEndpoint.Request<CommandDto, Command>.Response<ResponseDto, Response>.Mapper<Mapper>
    {
        public const string ERROR_MESSAGE = "Error-Message";

        public override Task<Result<Response>> ExecuteAsync(Command request, CancellationToken cancellationToken) =>
            Result.FailAsync<Response>(ERROR_MESSAGE);
    }

    public class Validator : AbstractValidator<CommandDto>
    {
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>, IWebApiEndpointResponseDtoMapper<Response, ResponseDto>
    {
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CommandDto dto, CancellationToken cancellationToken) =>
            new Command().ToResultOkAsync();

        public ResponseDto Map(Response domain) =>
            new (domain.FirstName, domain.Age);
    }

    [Fact]
    public async Task when_MetadataDefinition_is_null()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();
        
        var services = new ServiceCollection();
        services.AddSingleton<IWebApiEndpointLogger>(new Mock<IWebApiEndpointLogger>().Object);
        httpContext.RequestServices = services.BuildServiceProvider();

        var routePath = string.Empty;

        await WebApiEndpointExecutorService.ExecuteAsync(httpContext, null, routePath, CancellationToken.None);

        httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var streamReader = new StreamReader(httpContext.Response.Body);
        var requestBody = await streamReader.ReadToEndAsync();

        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        problemDetails.Title.Should().StartWith("WebApiEndpoint - Unable to find WebApiEndpoint for route");
    }

    [Fact]
    public async Task when_ApiEndpoint_call_succeeds()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IWebApiEndpointLogger>(new Mock<IWebApiEndpointLogger>().Object);
        services.AddSingleton<IWebApiEndpointHttpContextDispatcher, WebApiEndpointHttpContextDispatcher>();
        services.AddSingleton<IOptions<JsonOptions>>(Options.Create(new JsonOptions()));
        services.AddSingleton(typeof(IWebApiEndpointMiddlewareExecutor<,>), typeof(DisabledWebApiEndpointMiddlewareExecutor<,>));
        services.AddSingleton<ICommandWebApiEndpoint<RequestJsonDto<CommandDto>, ResponseJsonDto<ResponseDto>, Command, Response,
            RequestJsonMapper<CommandDto, Command, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>, SuccessApiEndpoint>();
        services.AddSingleton<WebApiEndpointDispatcher<RequestJsonDto<CommandDto>, ResponseJsonDto<ResponseDto>, Command, Response, RequestJsonMapper<CommandDto, Command, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>>();
        services.AddSingleton(typeof(IWebApiEndpointRequestValidation<>), typeof(WebApiEndpointRequestValidation<>));
        services.AddSingleton<IValidator<CommandDto>, Validator>();
        services.AddSingleton<Mapper>();
        services.AddSingleton<RequestJsonMapper<CommandDto, Command, Mapper>>();
        services.AddSingleton<ResponseJsonMapper<Response, ResponseDto, Mapper>>();
        services.AddSingleton<IRequestJsonReader<CommandDto>, RequestJsonReader<CommandDto>>();

        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();
        httpContext.RequestServices = services.BuildServiceProvider();

        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestJsonDto<CommandDto>), typeof(CommandDto), typeof(ResponseJsonDto<ResponseDto>), typeof(ResponseDto), typeof(SuccessApiEndpoint),
                                                                typeof(ICommandWebApiEndpoint<RequestJsonDto<CommandDto>, ResponseJsonDto<ResponseDto>, Command, Response, RequestJsonMapper<CommandDto, Command, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>),
                                                                typeof(IWebApiEndpointMiddlewareExecutor<Command, Response>),
                                                                typeof(WebApiEndpointDispatcher<RequestJsonDto<CommandDto>, ResponseJsonDto<ResponseDto>, Command, Response, RequestJsonMapper<CommandDto, Command, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>),
                                                                new List<Type>());

        var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
        var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());
        
        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

        var routePath = string.Empty;

        await WebApiEndpointExecutorService.ExecuteAsync(httpContext, metadataDefinition, routePath, CancellationToken.None);

        httpContext.Response.StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var streamReader = new StreamReader(httpContext.Response.Body);
        var requestBody = await streamReader.ReadToEndAsync();

        var response = JsonSerializer.Deserialize<ResponseDto>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        response.FirstName.Should().Be("FirstName");
        response.Age.Should().Be(10);
    }

    [Fact]
    public async Task when_ApiEndpoint_call_fails()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IWebApiEndpointLogger>(new Mock<IWebApiEndpointLogger>().Object);
        services.AddSingleton<IWebApiEndpointHttpContextDispatcher, WebApiEndpointHttpContextDispatcher>();
        services.AddSingleton<IOptions<JsonOptions>>(Options.Create(new JsonOptions()));
        services.AddSingleton(typeof(IWebApiEndpointMiddlewareExecutor<,>), typeof(DisabledWebApiEndpointMiddlewareExecutor<,>));
        services.AddSingleton<ICommandWebApiEndpoint<RequestJsonDto<CommandDto>, ResponseJsonDto<ResponseDto>, Command, Response,
            RequestJsonMapper<CommandDto, Command, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>, FailedApiEndpoint>();
        services.AddSingleton<WebApiEndpointDispatcher<RequestJsonDto<CommandDto>, ResponseJsonDto<ResponseDto>, Command, Response, RequestJsonMapper<CommandDto, Command, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>>();
        services.AddSingleton(typeof(IWebApiEndpointRequestValidation<>), typeof(WebApiEndpointRequestValidation<>));
        services.AddSingleton<IValidator<CommandDto>, Validator>();
        services.AddSingleton<Mapper>();
        services.AddSingleton<RequestJsonMapper<CommandDto, Command, Mapper>>();
        services.AddSingleton<ResponseJsonMapper<Response, ResponseDto, Mapper>>();
        services.AddSingleton<IRequestJsonReader<CommandDto>, RequestJsonReader<CommandDto>>();

        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();
        httpContext.RequestServices = services.BuildServiceProvider();

        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestJsonDto<CommandDto>), typeof(CommandDto), typeof(ResponseJsonDto<ResponseDto>), typeof(ResponseDto), typeof(FailedApiEndpoint),
                                                                typeof(ICommandWebApiEndpoint<RequestJsonDto<CommandDto>, ResponseJsonDto<ResponseDto>, Command, Response, RequestJsonMapper<CommandDto, Command, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>),
                                                                typeof(IWebApiEndpointMiddlewareExecutor<Command, Response>),
                                                                typeof(WebApiEndpointDispatcher<RequestJsonDto<CommandDto>, ResponseJsonDto<ResponseDto>, Command, Response, RequestJsonMapper<CommandDto, Command, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>),
                                                                new List<Type>());

        var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
        var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());

        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

        var routePath = string.Empty;

        await WebApiEndpointExecutorService.ExecuteAsync(httpContext, metadataDefinition, routePath, CancellationToken.None);

        httpContext.Response.StatusCode.Should().Be(metadataRouteDefinition.FailedStatusCode);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var streamReader = new StreamReader(httpContext.Response.Body);
        var requestBody = await streamReader.ReadToEndAsync();

        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        problemDetails.Title.Should().Be(FailedApiEndpoint.ERROR_MESSAGE);
        problemDetails.Detail.Should().Be(FailedApiEndpoint.ERROR_MESSAGE);
    }

    [Fact]
    public async Task when_unknown_error()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();
        
        var services = new ServiceCollection();
        services.AddSingleton<IWebApiEndpointLogger>(new Mock<IWebApiEndpointLogger>().Object);
        httpContext.RequestServices = services.BuildServiceProvider();

        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestJsonDto<CommandDto>), typeof(CommandDto), typeof(ResponseJsonDto<ResponseDto>), typeof(ResponseDto), typeof(SuccessApiEndpoint),
                                                                typeof(ICommandWebApiEndpoint<RequestJsonDto<CommandDto>, ResponseJsonDto<ResponseDto>, Command, Response, RequestJsonMapper<CommandDto, Command, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>),
                                                                typeof(IWebApiEndpointMiddlewareExecutor<Command, Response>),
                                                                typeof(WebApiEndpointDispatcher<RequestJsonDto<CommandDto>, ResponseJsonDto<ResponseDto>, Command, Response, RequestJsonMapper<CommandDto, Command, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>),
                                                                new List<Type>());

        var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
        var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());

        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

        var routePath = string.Empty;

        await WebApiEndpointExecutorService.ExecuteAsync(httpContext, metadataDefinition, routePath, CancellationToken.None);

        httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var streamReader = new StreamReader(httpContext.Response.Body);
        var requestBody = await streamReader.ReadToEndAsync();

        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        problemDetails.Title.Should().StartWith("WebApiEndpoint - Internal Server Error");
    }
}