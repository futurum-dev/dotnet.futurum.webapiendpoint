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
using Microsoft.Extensions.Options;

using Moq.AutoMock;

using Xunit;

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

        var result = await TestRunner(httpContext, apiEndpoint);

        result.ShouldBeSuccess();

        apiEndpoint.WasCalled.Should().BeTrue();

        httpContext.Response.StatusCode.Should().Be(MetadataRouteDefinition.FailedStatusCode);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var streamReader = new StreamReader(httpContext.Response.Body);
        var requestBody = await streamReader.ReadToEndAsync();

        var resultErrorStructure = JsonSerializer.Deserialize<ResultErrorStructure>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        resultErrorStructure.Message.Should().Be(ApiEndpoint.ErrorMessage);
        resultErrorStructure.Children.Should().BeEmpty();
    }

    private static async Task<Result> TestRunner(HttpContext httpContext, ApiEndpoint apiEndpoint)
    {
        var mocker = new AutoMocker();
        mocker.Use<IWebApiEndpointHttpContextDispatcher>(new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions())));
        mocker.Use<IWebApiEndpointRequestValidation<RequestDto>>(new WebApiEndpointRequestValidation<RequestDto>(EnumerableExtensions.Return(new Validator())));
        mocker.Use<IWebApiEndpointRequestMapper<RequestDto, Request>>(new Mapper());
        mocker.Use<IWebApiEndpointResponseDtoMapper<Response, ResponseDto>>(new Mapper());
        mocker.Use(new RequestJsonMapper<RequestDto, Request, Mapper>(new RequestJsonReader<RequestDto>(Options.Create(new JsonOptions())), new Mapper(), new WebApiEndpointRequestValidation<RequestDto>(EnumerableExtensions.Return(new Validator())) ));
        mocker.Use(new ResponseJsonMapper<Response, ResponseDto, Mapper>(Options.Create(new JsonOptions()), new Mapper()));

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

    private class ApiEndpoint : CommandWebApiEndpoint.WithRequest<RequestDto, Request>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
    {
        private readonly bool _isSuccess;

        public const string ErrorMessage = "ERROR-MESSAGE";

        public ApiEndpoint(bool isSuccess)
        {
            _isSuccess = isSuccess;
        }

        public bool WasCalled { get; private set; }

        protected override Task<Result<Response>> ExecuteAsync(Request query, CancellationToken cancellationToken)
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
//
// public class CommandWebApiEndpointDispatcherTests
// {
//     private readonly ITestOutputHelper _output;
//
//     public CommandWebApiEndpointDispatcherTests(ITestOutputHelper output)
//     {
//         _output = output;
//     }
//
//     public class Validator : AbstractValidator<CommandDto>
//     {
//     }
//
//     public record CommandDto;
//
//     public record Command;
//
//     public record ResponseDto;
//
//     public record Response;
//
//     private static readonly MetadataRouteDefinition MetadataRouteDefinition =
//         new(MetadataRouteHttpMethod.Post, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, Option<Action<RouteHandlerBuilder>>.None, null);
//
//     public class CommandWithRequestWithResponse
//     {
//         [Fact]
//         public async Task success()
//         {
//             var apiEndpoint = new ApiEndpoint(true);
//             var httpContext = new DefaultHttpContext();
//             var result = await TestRunner(httpContext, apiEndpoint);
//
//             result.ShouldBeSuccess();
//
//             apiEndpoint.WasCalled.Should().BeTrue();
//
//             httpContext.Response.StatusCode.Should().Be(MetadataRouteDefinition.SuccessStatusCode);
//         }
//
//         [Fact]
//         public async Task failure()
//         {
//             var apiEndpoint = new ApiEndpoint(false);
//
//             var httpContext = new DefaultHttpContext();
//             httpContext.Response.Body = new MemoryStream();
//
//             var result = await TestRunner(httpContext, apiEndpoint);
//
//             result.ShouldBeSuccess();
//
//             apiEndpoint.WasCalled.Should().BeTrue();
//
//             httpContext.Response.StatusCode.Should().Be(MetadataRouteDefinition.FailedStatusCode);
//
//             httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
//             using var streamReader = new StreamReader(httpContext.Response.Body);
//             var requestBody = await streamReader.ReadToEndAsync();
//
//             var resultErrorStructure = JsonSerializer.Deserialize<ResultErrorStructure>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));
//
//             resultErrorStructure.Message.Should().Be(ApiEndpoint.ErrorMessage);
//             resultErrorStructure.Children.Should().BeEmpty();
//         }
//
//         private static async Task<Result> TestRunner(HttpContext httpContext, ApiEndpoint apiEndpoint)
//         {
//             var mocker = new AutoMocker();
//             mocker.Use<IWebApiEndpointHttpContextDispatcher>(new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions())));
//             mocker.Use<IWebApiEndpointRequestValidation<CommandDto>>(new WebApiEndpointRequestValidation<CommandDto>(EnumerableExtensions.Return(new Validator())));
//             mocker.Use<IWebApiEndpointRequestMapper<CommandDto, Command>>(new Mapper());
//             mocker.Use<IWebApiEndpointResponseMapper<Response, ResponseDto>>(new Mapper());
//
//             var metadataTypeDefinition = new MetadataTypeDefinition(typeof(CommandDto), typeof(ResponseDto), typeof(ApiEndpoint),
//                                                                     typeof(ICommandWebApiEndpoint<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>),
//                                                                     typeof(IWebApiEndpointMiddlewareExecutor<Command, Response>),
//                                                                     typeof(CommandWebApiEndpointDispatcher<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>),
//                                                                     new List<Type>());
//             var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
//             var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());
//             var metadataDefinition = new MetadataDefinition(MetadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);
//
//             var middlewareExecutor = new DisabledWebApiEndpointMiddlewareExecutor<Command, Response>();
//
//             var commandWebApiEndpointDispatcher = mocker.CreateInstance<CommandWebApiEndpointDispatcher<CommandDto, ResponseDto, Command, Response, Mapper, Mapper>>();
//
//             return await commandWebApiEndpointDispatcher.ExecuteAsync(metadataDefinition, httpContext, middlewareExecutor, apiEndpoint, CancellationToken.None);
//         }
//
//         private class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
//         {
//             private readonly bool _isSuccess;
//
//             public const string ErrorMessage = "ERROR-MESSAGE";
//
//             public ApiEndpoint(bool isSuccess)
//             {
//                 _isSuccess = isSuccess;
//             }
//
//             public bool WasCalled { get; private set; }
//
//             protected override Task<Result<Response>> ExecuteAsync(Command query, CancellationToken cancellationToken)
//             {
//                 WasCalled = true;
//
//                 return _isSuccess
//                     ? new Response().ToResultOkAsync()
//                     : Result.FailAsync<Response>(ErrorMessage);
//             }
//         }
//
//         public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>, IWebApiEndpointResponseMapper<Response, ResponseDto>
//         {
//             public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CommandDto dto, CancellationToken cancellationToken) =>
//                 new Command().ToResultOkAsync();
//
//             public Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, Response domain, CancellationToken cancellation) => 
//                 Task.FromResult(new ResponseDto());
//         }
//     }
//
//     public class CommandWithoutRequestWithResponse
//     {
//         [Fact]
//         public async Task success()
//         {
//             var apiEndpoint = new ApiEndpoint(true);
//             var httpContext = new DefaultHttpContext();
//             var result = await TestRunner(httpContext, apiEndpoint);
//
//             result.ShouldBeSuccess();
//
//             apiEndpoint.WasCalled.Should().BeTrue();
//
//             httpContext.Response.StatusCode.Should().Be(MetadataRouteDefinition.SuccessStatusCode);
//         }
//
//         [Fact]
//         public async Task failure()
//         {
//             var apiEndpoint = new ApiEndpoint(false);
//
//             var httpContext = new DefaultHttpContext();
//             httpContext.Response.Body = new MemoryStream();
//
//             var result = await TestRunner(httpContext, apiEndpoint);
//
//             result.ShouldBeSuccess();
//
//             apiEndpoint.WasCalled.Should().BeTrue();
//
//             httpContext.Response.StatusCode.Should().Be(MetadataRouteDefinition.FailedStatusCode);
//
//             httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
//             using var streamReader = new StreamReader(httpContext.Response.Body);
//             var requestBody = await streamReader.ReadToEndAsync();
//
//             var resultErrorStructure = JsonSerializer.Deserialize<ResultErrorStructure>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));
//
//             resultErrorStructure.Message.Should().Be(ApiEndpoint.ErrorMessage);
//             resultErrorStructure.Children.Should().BeEmpty();
//         }
//
//         private static async Task<Result> TestRunner(HttpContext httpContext, ApiEndpoint apiEndpoint)
//         {
//             var mocker = new AutoMocker();
//             mocker.Use<IWebApiEndpointHttpContextDispatcher>(new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions())));
//             mocker.Use<IWebApiEndpointRequestValidation<CommandDto>>(new WebApiEndpointRequestValidation<CommandDto>(EnumerableExtensions.Return(new Validator())));
//             mocker.Use<IWebApiEndpointRequestMapper<Command>>(new Mapper());
//             mocker.Use<IWebApiEndpointResponseMapper<Response, ResponseDto>>(new Mapper());
//
//             var metadataTypeDefinition = new MetadataTypeDefinition(typeof(EmptyRequestDto), typeof(ResponseDto), typeof(ApiEndpoint),
//                                                                     typeof(ICommandWebApiEndpoint<ResponseDto, Command, Response, Mapper, Mapper>),
//                                                                     typeof(IWebApiEndpointMiddlewareExecutor<Command, Response>),
//                                                                     typeof(CommandWebApiEndpointDispatcher<ResponseDto, Command, Response, Mapper, Mapper>),
//                                                                     new List<Type>());
//             var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
//             var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());
//             var metadataDefinition = new MetadataDefinition(MetadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);
//
//             var middlewareExecutor = new DisabledWebApiEndpointMiddlewareExecutor<Command, Response>();
//
//             var commandWebApiEndpointDispatcher = mocker.CreateInstance<CommandWebApiEndpointDispatcher<ResponseDto, Command, Response, Mapper, Mapper>>();
//
//             return await commandWebApiEndpointDispatcher.ExecuteAsync(metadataDefinition, httpContext, middlewareExecutor, apiEndpoint, CancellationToken.None);
//         }
//
//         private class ApiEndpoint : CommandWebApiEndpoint.WithRequest<Command>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
//         {
//             private readonly bool _isSuccess;
//
//             public const string ErrorMessage = "ERROR-MESSAGE";
//
//             public ApiEndpoint(bool isSuccess)
//             {
//                 _isSuccess = isSuccess;
//             }
//
//             public bool WasCalled { get; private set; }
//
//             protected override Task<Result<Response>> ExecuteAsync(Command query, CancellationToken cancellationToken)
//             {
//                 WasCalled = true;
//
//                 return _isSuccess
//                     ? new Response().ToResultOkAsync()
//                     : Result.FailAsync<Response>(ErrorMessage);
//             }
//         }
//
//         public class Mapper : IWebApiEndpointRequestMapper<Command>, IWebApiEndpointResponseMapper<Response, ResponseDto>
//         {
//             public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
//                 new Command().ToResultOkAsync();
//
//             public Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, Response domain, CancellationToken cancellation) => 
//                 Task.FromResult(new ResponseDto());
//         }
//     }
//
//     public class CommandWithoutResponse
//     {
//         [Fact]
//         public async Task success()
//         {
//             var apiEndpoint = new ApiEndpoint(true);
//             var httpContext = new DefaultHttpContext();
//             var result = await TestRunner(httpContext, apiEndpoint);
//
//             result.ShouldBeSuccess();
//
//             apiEndpoint.WasCalled.Should().BeTrue();
//
//             httpContext.Response.StatusCode.Should().Be(MetadataRouteDefinition.SuccessStatusCode);
//         }
//
//         [Fact]
//         public async Task failure()
//         {
//             var apiEndpoint = new ApiEndpoint(false);
//
//             var httpContext = new DefaultHttpContext();
//             httpContext.Response.Body = new MemoryStream();
//
//             var result = await TestRunner(httpContext, apiEndpoint);
//
//             result.ShouldBeSuccess();
//
//             apiEndpoint.WasCalled.Should().BeTrue();
//
//             httpContext.Response.StatusCode.Should().Be(MetadataRouteDefinition.FailedStatusCode);
//
//             httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
//             using var streamReader = new StreamReader(httpContext.Response.Body);
//             var requestBody = await streamReader.ReadToEndAsync();
//
//             var resultErrorStructure = JsonSerializer.Deserialize<ResultErrorStructure>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));
//
//             resultErrorStructure.Message.Should().Be(ApiEndpoint.ErrorMessage);
//             resultErrorStructure.Children.Should().BeEmpty();
//         }
//
//         private static async Task<Result> TestRunner(HttpContext httpContext, ApiEndpoint apiEndpoint)
//         {
//             var mocker = new AutoMocker();
//             mocker.Use<IWebApiEndpointHttpContextDispatcher>(new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions())));
//             mocker.Use<IWebApiEndpointRequestValidation<CommandDto>>(new WebApiEndpointRequestValidation<CommandDto>(EnumerableExtensions.Return(new Validator())));
//             mocker.Use<IWebApiEndpointRequestMapper<CommandDto, Command>>(new Mapper());
//
//             var metadataTypeDefinition = new MetadataTypeDefinition(typeof(CommandDto), typeof(EmptyResponseDto), typeof(ApiEndpoint),
//                                                                     typeof(ICommandWebApiEndpoint<CommandDto, Command, Mapper>),
//                                                                     typeof(IWebApiEndpointMiddlewareExecutor<Command, Response>),
//                                                                     typeof(CommandWebApiEndpointDispatcher<CommandDto, Command, Mapper>),
//                                                                     new List<Type>());
//             var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
//             var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());
//             var metadataDefinition = new MetadataDefinition(MetadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);
//
//             var middlewareExecutor = new DisabledWebApiEndpointMiddlewareExecutor<Command, Unit>();
//
//             var commandWebApiEndpointDispatcher = mocker.CreateInstance<CommandWebApiEndpointDispatcher<CommandDto, Command, Mapper>>();
//
//             return await commandWebApiEndpointDispatcher.ExecuteAsync(metadataDefinition, httpContext, middlewareExecutor, apiEndpoint, CancellationToken.None);
//         }
//
//         private class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithoutResponse.WithMapper<Mapper>
//         {
//             private readonly bool _isSuccess;
//
//             public const string ErrorMessage = "ERROR-MESSAGE";
//
//             public ApiEndpoint(bool isSuccess)
//             {
//                 _isSuccess = isSuccess;
//             }
//
//             public bool WasCalled { get; private set; }
//
//             protected override Task<Result> ExecuteAsync(Command query, CancellationToken cancellationToken)
//             {
//                 WasCalled = true;
//
//                 return _isSuccess
//                     ? Result.OkAsync()
//                     : Result.FailAsync(ErrorMessage);
//             }
//         }
//
//         public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
//         {
//             public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CommandDto dto, CancellationToken cancellationToken) =>
//                 new Command().ToResultOkAsync();
//         }
//     }
//
//     public class CommandWithoutRequestWithoutResponse
//     {
//         [Fact]
//         public async Task success()
//         {
//             var apiEndpoint = new ApiEndpoint(true);
//             var httpContext = new DefaultHttpContext();
//             var result = await TestRunner(httpContext, apiEndpoint);
//
//             result.ShouldBeSuccess();
//
//             apiEndpoint.WasCalled.Should().BeTrue();
//
//             httpContext.Response.StatusCode.Should().Be(MetadataRouteDefinition.SuccessStatusCode);
//         }
//
//         [Fact]
//         public async Task failure()
//         {
//             var apiEndpoint = new ApiEndpoint(false);
//
//             var httpContext = new DefaultHttpContext();
//             httpContext.Response.Body = new MemoryStream();
//
//             var result = await TestRunner(httpContext, apiEndpoint);
//
//             result.ShouldBeSuccess();
//
//             apiEndpoint.WasCalled.Should().BeTrue();
//
//             httpContext.Response.StatusCode.Should().Be(MetadataRouteDefinition.FailedStatusCode);
//
//             httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
//             using var streamReader = new StreamReader(httpContext.Response.Body);
//             var requestBody = await streamReader.ReadToEndAsync();
//
//             var resultErrorStructure = JsonSerializer.Deserialize<ResultErrorStructure>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));
//
//             resultErrorStructure.Message.Should().Be(ApiEndpoint.ErrorMessage);
//             resultErrorStructure.Children.Should().BeEmpty();
//         }
//
//         private static async Task<Result> TestRunner(HttpContext httpContext, ApiEndpoint apiEndpoint)
//         {
//             var mocker = new AutoMocker();
//             mocker.Use<IWebApiEndpointHttpContextDispatcher>(new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions())));
//             mocker.Use<IWebApiEndpointRequestMapper<Command>>(new Mapper());
//
//
//             var metadataTypeDefinition = new MetadataTypeDefinition(typeof(EmptyRequestDto), typeof(EmptyResponseDto), typeof(ApiEndpoint),
//                                                                     typeof(ICommandWebApiEndpoint<Command, Mapper>),
//                                                                     typeof(IWebApiEndpointMiddlewareExecutor<Command, Response>),
//                                                                     typeof(CommandWebApiEndpointDispatcher<Command, Mapper>),
//                                                                     new List<Type>());
//             var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
//             var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());
//             var metadataDefinition = new MetadataDefinition(MetadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);
//
//             var middlewareExecutor = new DisabledWebApiEndpointMiddlewareExecutor<Command, Unit>();
//
//             var commandWebApiEndpointDispatcher = mocker.CreateInstance<CommandWebApiEndpointDispatcher<Command, Mapper>>();
//
//             return await commandWebApiEndpointDispatcher.ExecuteAsync(metadataDefinition, httpContext, middlewareExecutor, apiEndpoint, CancellationToken.None);
//         }
//
//         private class ApiEndpoint : CommandWebApiEndpoint.WithRequest<Command>.WithoutResponse.WithMapper<Mapper>
//         {
//             private readonly bool _isSuccess;
//
//             public const string ErrorMessage = "ERROR-MESSAGE";
//
//             public ApiEndpoint(bool isSuccess)
//             {
//                 _isSuccess = isSuccess;
//             }
//
//             public bool WasCalled { get; private set; }
//
//             protected override Task<Result> ExecuteAsync(Command query, CancellationToken cancellationToken)
//             {
//                 WasCalled = true;
//
//                 return _isSuccess
//                     ? Result.OkAsync()
//                     : Result.FailAsync(ErrorMessage);
//             }
//         }
//
//         public class Mapper : IWebApiEndpointRequestMapper<Command>
//         {
//             public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
//                 new Command().ToResultOkAsync();
//         }
//     }
// }