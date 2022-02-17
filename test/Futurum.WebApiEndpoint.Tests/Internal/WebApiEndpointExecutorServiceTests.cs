using System.Net;
using System.Text.Json;

using FluentAssertions;

using FluentValidation;

using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Internal.Dispatcher;
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

    private class SuccessApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
    {
        protected override Task<Result<Response>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            new Response("FirstName", 10).ToResultOkAsync();
    }

    private class FailedApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
    {
        public const string ERROR_MESSAGE = "Error-Message";

        protected override Task<Result<Response>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            Result.FailAsync<Response>(ERROR_MESSAGE);
    }

    public class Validator : AbstractValidator<CommandDto>
    {
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>, IWebApiEndpointResponseMapper<Response, ResponseDto>
    {
        public Result<Command> Map(HttpContext httpContext, CommandDto dto) =>
            new Command().ToResultOk();

        public Result<ResponseDto> Map(Response domain) =>
            new ResponseDto(domain.FirstName, domain.Age).ToResultOk();
    }

    [Fact]
    public async Task when_MetadataDefinition_is_null()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();

        var routePath = string.Empty;

        await WebApiEndpointExecutorService.ExecuteAsync(httpContext, null, WebApiEndpointConfiguration.Default, routePath, CancellationToken.None);

        httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var streamReader = new StreamReader(httpContext.Response.Body);
        var requestBody = await streamReader.ReadToEndAsync();

        var resultErrorStructure = JsonSerializer.Deserialize<ResultErrorStructure>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        resultErrorStructure.Message.Should().StartWith("WebApiEndpoint - Unable to find WebApiEndpoint for route");
        resultErrorStructure.Children.Should().BeEmpty();
    }

    [Fact]
    public async Task when_ApiEndpoint_call_succeeds()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IWebApiEndpointLogger>(new Mock<IWebApiEndpointLogger>().Object);
        services.AddSingleton<IWebApiEndpointHttpContextDispatcher, WebApiEndpointHttpContextDispatcher>();
        services.AddSingleton<IOptions<JsonOptions>>(Options.Create(new JsonOptions()));
        services.AddSingleton(typeof(IWebApiEndpointMiddlewareExecutor<,>), typeof(DisabledWebApiEndpointMiddlewareExecutor<,>));
        services.AddSingleton<ICommandWebApiEndpoint<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>, SuccessApiEndpoint>();
        services.AddSingleton<CommandWebApiEndpointDispatcher<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>>();
        services.AddSingleton(typeof(IWebApiEndpointRequestValidation<>), typeof(WebApiEndpointRequestValidation<>));
        services.AddSingleton<IValidator<CommandDto>, Validator>();
        services.AddSingleton<Mapper>();
        services.AddSingleton<Mapper>();

        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();
        httpContext.RequestServices = services.BuildServiceProvider();

        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataTypeDefinition = new MetadataTypeDefinition(typeof(CommandDto), typeof(ResponseDto), typeof(SuccessApiEndpoint),
                                                                typeof(ICommandWebApiEndpoint<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>),
                                                                typeof(IWebApiEndpointMiddlewareExecutor<Command, Response>),
                                                                typeof(CommandWebApiEndpointDispatcher<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>),
                                                                new List<Type>());

        var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());

        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition);

        var routePath = string.Empty;

        await WebApiEndpointExecutorService.ExecuteAsync(httpContext, metadataDefinition, WebApiEndpointConfiguration.Default, routePath, CancellationToken.None);

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
        services.AddSingleton<ICommandWebApiEndpoint<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>, FailedApiEndpoint>();
        services.AddSingleton<CommandWebApiEndpointDispatcher<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>>();
        services.AddSingleton(typeof(IWebApiEndpointRequestValidation<>), typeof(WebApiEndpointRequestValidation<>));
        services.AddSingleton<IValidator<CommandDto>, Validator>();
        services.AddSingleton<Mapper>();
        services.AddSingleton<Mapper>();

        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();
        httpContext.RequestServices = services.BuildServiceProvider();

        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataTypeDefinition = new MetadataTypeDefinition(typeof(CommandDto), typeof(ResponseDto), typeof(SuccessApiEndpoint),
                                                                typeof(ICommandWebApiEndpoint<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>),
                                                                typeof(IWebApiEndpointMiddlewareExecutor<Command, Response>),
                                                                typeof(CommandWebApiEndpointDispatcher<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>),
                                                                new List<Type>());

        var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());

        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition);

        var routePath = string.Empty;

        await WebApiEndpointExecutorService.ExecuteAsync(httpContext, metadataDefinition, WebApiEndpointConfiguration.Default, routePath, CancellationToken.None);

        httpContext.Response.StatusCode.Should().Be(metadataRouteDefinition.FailedStatusCode);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var streamReader = new StreamReader(httpContext.Response.Body);
        var requestBody = await streamReader.ReadToEndAsync();

        var resultErrorStructure = JsonSerializer.Deserialize<ResultErrorStructure>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        resultErrorStructure.Message.Should().Be(FailedApiEndpoint.ERROR_MESSAGE);
        resultErrorStructure.Children.Should().BeEmpty();
    }

    [Fact]
    public async Task when_unknown_error()
    {
        var services = new ServiceCollection();

        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();
        httpContext.RequestServices = services.BuildServiceProvider();

        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataTypeDefinition = new MetadataTypeDefinition(typeof(CommandDto), typeof(ResponseDto), typeof(SuccessApiEndpoint),
                                                                typeof(ICommandWebApiEndpoint<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>),
                                                                typeof(IWebApiEndpointMiddlewareExecutor<Command, Response>),
                                                                typeof(CommandWebApiEndpointDispatcher<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>),
                                                                new List<Type>());

        var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());

        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition);

        var routePath = string.Empty;

        await WebApiEndpointExecutorService.ExecuteAsync(httpContext, metadataDefinition, WebApiEndpointConfiguration.Default, routePath, CancellationToken.None);

        httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var streamReader = new StreamReader(httpContext.Response.Body);
        var requestBody = await streamReader.ReadToEndAsync();

        var resultErrorStructure = JsonSerializer.Deserialize<ResultErrorStructure>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        resultErrorStructure.Message.Should().StartWith("WebApiEndpoint error for route");
        resultErrorStructure.Children.Should().BeEmpty();
    }
}