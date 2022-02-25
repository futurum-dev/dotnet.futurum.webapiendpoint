using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

using FluentAssertions;

using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.Test.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

using Xunit;
using Xunit.Abstractions;

namespace Futurum.WebApiEndpoint.Tests.Internal;

public class WebApiEndpointHttpContextDispatcherTests
{
    private readonly ITestOutputHelper _output;

    public WebApiEndpointHttpContextDispatcherTests(ITestOutputHelper output)
    {
        _output = output;
    }

    public class ReadRequestAsync
    {
        public class RequestPlainText
        {
            [Fact]
            public async Task success()
            {
                var message = Guid.NewGuid().ToString();

                var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

                var httpContext = new DefaultHttpContext();
                httpContext.Request.Body = new MemoryStream(Encoding.Default.GetBytes(message));

                var result = await webApiEndpointHttpContextDispatcher.ReadRequestAsync<RequestPlainTextDto>(httpContext, null, null, CancellationToken.None);

                result.ShouldBeSuccessWithValue(new RequestPlainTextDto(message));
            }
        }

        public class RequestUploadFiles
        {
            [Fact]
            public async Task success()
            {
                await using var fileStream = File.OpenRead("./Data/hello-world.txt");

                var formFile = new FormFile(fileStream, 0, fileStream.Length, "hello-world.txt", "hello-world.txt");

                var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

                var httpContext = new DefaultHttpContext();
                httpContext.Request.Form = new FormCollection(null, new FormFileCollection { formFile });

                var result = await webApiEndpointHttpContextDispatcher.ReadRequestAsync<RequestUploadFilesDto>(httpContext, null, null, CancellationToken.None);

                result.ShouldBeSuccessWithValueAssertion(x =>
                {
                    x.Files.Count().Should().Be(1);

                    var formFile = x.Files.Single();

                    formFile.Name.Should().Be("hello-world.txt");
                    formFile.FileName.Should().Be("hello-world.txt");
                    formFile.Length.Should().Be(fileStream.Length);
                });
            }

            [Fact]
            public async Task failure()
            {
                var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

                var httpContext = new DefaultHttpContext();

                var result = await webApiEndpointHttpContextDispatcher.ReadRequestAsync<RequestUploadFilesDto>(httpContext, null, null, CancellationToken.None);

                result.ShouldBeFailureWithError("Failed to read upload files");
            }
        }

        public class RequestUploadFile
        {
            [Fact]
            public async Task success()
            {
                await using var fileStream = File.OpenRead("./Data/hello-world.txt");

                var formFile = new FormFile(fileStream, 0, fileStream.Length, "hello-world.txt", "hello-world.txt");

                var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

                var httpContext = new DefaultHttpContext();
                httpContext.Request.Form = new FormCollection(null, new FormFileCollection { formFile });

                var result = await webApiEndpointHttpContextDispatcher.ReadRequestAsync<RequestUploadFileDto>(httpContext, null, null, CancellationToken.None);

                result.ShouldBeSuccessWithValueAssertion(x =>
                {
                    x.File.Should().NotBeNull();

                    x.File.Name.Should().Be("hello-world.txt");
                    x.File.FileName.Should().Be("hello-world.txt");
                    x.File.Length.Should().Be(fileStream.Length);
                });
            }

            [Fact]
            public async Task failure()
            {
                var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

                var httpContext = new DefaultHttpContext();

                var result = await webApiEndpointHttpContextDispatcher.ReadRequestAsync<RequestUploadFileDto>(httpContext, null, null, CancellationToken.None);

                result.ShouldBeFailureWithErrorContaining("Failed to read upload file");
            }
        }

        public class Json
        {
            public record RequestDto(string FirstName, int Age);

            [Fact]
            public async Task success()
            {
                var firstName = Guid.NewGuid().ToString();
                var age = 10;

                var requestDto = new RequestDto(firstName, age);

                var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

                var httpContext = new DefaultHttpContext();

                var json = JsonSerializer.Serialize(requestDto);
                var stream = new MemoryStream(Encoding.Default.GetBytes(json));
                httpContext.Request.Body = stream;
                httpContext.Request.ContentLength = stream.Length;
                httpContext.Request.ContentType = MediaTypeNames.Application.Json;

                var result = await webApiEndpointHttpContextDispatcher.ReadRequestAsync<RequestDto>(httpContext, null, null, CancellationToken.None);

                result.ShouldBeSuccessWithValue(requestDto);
            }

            [Fact]
            public async Task failure()
            {
                var firstName = Guid.NewGuid().ToString();
                var age = 10;

                var requestDto = new RequestDto(firstName, age);

                var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

                var httpContext = new DefaultHttpContext();

                var json = JsonSerializer.Serialize(requestDto);
                var stream = new MemoryStream(Encoding.Default.GetBytes(json));
                httpContext.Request.Body = stream;
                httpContext.Request.ContentLength = stream.Length;
                httpContext.Request.ContentType = MediaTypeNames.Application.Json;

                var result = await webApiEndpointHttpContextDispatcher.ReadRequestAsync<FailingDeserializeObject>(httpContext, null, null, CancellationToken.None);

                result.ShouldBeFailureWithErrorContaining("Failed to deserialize request as json");
            }

            public record FailingDeserializeObject(int FirstName);
        }

        public class JsonAndMapFrom
        {
            public record RequestDto(string FirstName, int Age)
            {
                [MapFromPath("Id")] public string Id { get; set; }
            }

            [Fact]
            public async Task success()
            {
                var firstName = Guid.NewGuid().ToString();
                var age = 10;
                var idValue = Guid.NewGuid().ToString();

                var requestDto = new RequestDto(firstName, age);

                var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

                var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());

                var httpContext = new DefaultHttpContext();

                httpContext.Request.RouteValues = new RouteValueDictionary(new Dictionary<string, string> { { "Id", idValue } });

                var json = JsonSerializer.Serialize(requestDto);
                var stream = new MemoryStream(Encoding.Default.GetBytes(json));
                httpContext.Request.Body = stream;
                httpContext.Request.ContentLength = stream.Length;
                httpContext.Request.ContentType = MediaTypeNames.Application.Json;

                var result = await webApiEndpointHttpContextDispatcher.ReadRequestAsync<RequestDto>(httpContext, metadataMapFromDefinition, null, CancellationToken.None);

                result.ShouldBeSuccessWithValue(new RequestDto(firstName, age) { Id = idValue });
            }
        }

        public class MapFrom
        {
            public record RequestDto
            {
                [MapFromPath("Id")] public string Id { get; set; }
            }

            [Fact]
            public async Task success()
            {
                var value = Guid.NewGuid().ToString();

                var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

                var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());

                var httpContext = new DefaultHttpContext();
                httpContext.Request.RouteValues = new RouteValueDictionary(new Dictionary<string, string> { { "Id", value } });

                var result = await webApiEndpointHttpContextDispatcher.ReadRequestAsync<RequestDto>(httpContext, metadataMapFromDefinition, null, CancellationToken.None);

                result.ShouldBeSuccessWithValue(new RequestDto { Id = value });
            }
        }

        public class MapFromMultipart
        {
            [Fact]
            public void success()
            {
                // Yet to figure out how to unit test this, but it is covered by an end-to-end test
            }

            [Fact]
            public void failure()
            {
                // Yet to figure out how to unit test this, but it is covered by an end-to-end test
            }
        }

        public class EmptyRequest
        {
            [Fact]
            public async Task success()
            {
                var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

                var httpContext = new DefaultHttpContext();

                var result = await webApiEndpointHttpContextDispatcher.ReadRequestAsync<EmptyRequestDto>(httpContext, null, null, CancellationToken.None);

                result.ShouldBeSuccessWithValue(new EmptyRequestDto());
            }
        }
    }

    public class HandleSuccessResponseAsyncWithoutResponse
    {
        public record RequestDto;

        [Fact]
        public async Task success()
        {
            MetadataRouteDefinition metadataRouteDefinition =
                new(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, Option<Action<RouteHandlerBuilder>>.None, null);

            var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

            var httpContext = new DefaultHttpContext();

            var result = await webApiEndpointHttpContextDispatcher.HandleSuccessResponseAsync(httpContext, metadataRouteDefinition, CancellationToken.None);

            result.ShouldBeSuccess();

            httpContext.Response.StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);
        }
    }

    public class HandleSuccessResponseAsyncWithResponse
    {
        public class ResponseStream
        {
            [Fact]
            public async Task success()
            {
                var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

                var httpContext = new DefaultHttpContext();
                httpContext.Request.Body = new MemoryStream();
                httpContext.Response.Body = new MemoryStream();

                var fileStream = new FileInfo("./Data/hello-world.txt").OpenRead();

                var sentBytes = new byte[fileStream.Length];
                await fileStream.ReadAsync(sentBytes, CancellationToken.None);

                var responseDto = new ResponseStreamDto(new FileInfo("./Data/hello-world.txt").OpenRead(), MediaTypeNames.Application.Octet);

                MetadataRouteDefinition metadataRouteDefinition =
                    new(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, Option<Action<RouteHandlerBuilder>>.None, null);

                var result = await webApiEndpointHttpContextDispatcher.HandleSuccessResponseAsync(httpContext, responseDto, metadataRouteDefinition, CancellationToken.None);

                result.ShouldBeSuccess();

                httpContext.Response.StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);

                httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
                using var streamReader = new StreamReader(httpContext.Response.Body);
                var receivedBytes = new byte[fileStream.Length];

                await httpContext.Response.Body.ReadAsync(receivedBytes, CancellationToken.None);

                receivedBytes.Should().BeEquivalentTo(sentBytes);
            }
        }

        public class ResponseFileStream
        {
            [Fact]
            public async Task success()
            {
                var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

                var httpContext = new DefaultHttpContext();
                httpContext.Request.Body = new MemoryStream();
                httpContext.Response.Body = new MemoryStream();

                var fileStream = new FileInfo("./Data/hello-world.txt").OpenRead();

                var sentBytes = new byte[fileStream.Length];
                await fileStream.ReadAsync(sentBytes, CancellationToken.None);

                var responseDto = new ResponseFileStreamDto(new FileInfo("./Data/hello-world.txt"), MediaTypeNames.Application.Octet);

                MetadataRouteDefinition metadataRouteDefinition =
                    new(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, Option<Action<RouteHandlerBuilder>>.None, null);

                var result = await webApiEndpointHttpContextDispatcher.HandleSuccessResponseAsync(httpContext, responseDto, metadataRouteDefinition, CancellationToken.None);

                result.ShouldBeSuccess();

                httpContext.Response.StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);

                httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
                using var streamReader = new StreamReader(httpContext.Response.Body);
                var receivedBytes = new byte[fileStream.Length];

                await httpContext.Response.Body.ReadAsync(receivedBytes, CancellationToken.None);

                receivedBytes.Should().BeEquivalentTo(sentBytes);
            }
        }

        public class ResponseBytes
        {
            [Fact]
            public async Task success()
            {
                var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

                var httpContext = new DefaultHttpContext();
                httpContext.Request.Body = new MemoryStream();
                httpContext.Response.Body = new MemoryStream();

                var fileStream = new FileInfo("./Data/hello-world.txt").OpenRead();

                var sentBytes = new byte[fileStream.Length];
                await fileStream.ReadAsync(sentBytes, CancellationToken.None);

                var responseDto = new ResponseBytesDto(sentBytes, MediaTypeNames.Application.Octet);

                MetadataRouteDefinition metadataRouteDefinition =
                    new(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, Option<Action<RouteHandlerBuilder>>.None, null);

                var result = await webApiEndpointHttpContextDispatcher.HandleSuccessResponseAsync(httpContext, responseDto, metadataRouteDefinition, CancellationToken.None);

                result.ShouldBeSuccess();

                httpContext.Response.StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);

                httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
                using var streamReader = new StreamReader(httpContext.Response.Body);
                var receivedBytes = new byte[fileStream.Length];

                await httpContext.Response.Body.ReadAsync(receivedBytes, CancellationToken.None);

                receivedBytes.Should().BeEquivalentTo(sentBytes);
            }
        }

        public class ResponseAsyncEnumerable
        {
            [Fact]
            public async Task success()
            {
                var numbers = Enumerable.Range(0, 100)
                                        .ToList();

                var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

                var httpContext = new DefaultHttpContext();
                httpContext.Request.Body = new MemoryStream();
                httpContext.Response.Body = new MemoryStream();

                var responseDto = new ResponseAsyncEnumerableDto<int>(AsyncEnumerable(numbers));

                MetadataRouteDefinition metadataRouteDefinition =
                    new(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, Option<Action<RouteHandlerBuilder>>.None, null);

                var result = await webApiEndpointHttpContextDispatcher.HandleSuccessResponseAsync(httpContext, responseDto, metadataRouteDefinition, CancellationToken.None);

                result.ShouldBeSuccess();

                httpContext.Response.StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);

                httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
                using var streamReader = new StreamReader(httpContext.Response.Body);
                var requestBody = await streamReader.ReadToEndAsync();

                var receivedNumbers = JsonSerializer.Deserialize<IEnumerable<int>>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

                receivedNumbers.Should().BeEquivalentTo(numbers);
            }

            private static async IAsyncEnumerable<int> AsyncEnumerable(IEnumerable<int> numbers)
            {
                await Task.Yield();

                foreach (var number in numbers)
                {
                    yield return number;
                }
            }
        }

        public class ResponseEmptyJson
        {
            [Fact]
            public async Task success()
            {
                var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

                var httpContext = new DefaultHttpContext();
                httpContext.Request.Body = new MemoryStream();
                httpContext.Response.Body = new MemoryStream();

                var responseDto = new ResponseEmptyJsonDto();

                MetadataRouteDefinition metadataRouteDefinition =
                    new(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, Option<Action<RouteHandlerBuilder>>.None, null);

                var result = await webApiEndpointHttpContextDispatcher.HandleSuccessResponseAsync(httpContext, responseDto, metadataRouteDefinition, CancellationToken.None);

                result.ShouldBeSuccess();

                httpContext.Response.StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);

                httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
                using var streamReader = new StreamReader(httpContext.Response.Body);
                var requestBody = await streamReader.ReadToEndAsync();

                var receivedEmptyJson = JsonSerializer.Deserialize<JsonObject>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

                receivedEmptyJson.Should().NotBeNull();
                receivedEmptyJson.ToJsonString().Should().Be("{}");
            }
        }

        public class Json
        {
            public record ResponseDto(string FirstName, int Age);

            [Fact]
            public async Task success()
            {
                var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

                var httpContext = new DefaultHttpContext();
                httpContext.Request.Body = new MemoryStream();
                httpContext.Response.Body = new MemoryStream();

                var responseDto = new ResponseDto(Guid.NewGuid().ToString(), 10);

                MetadataRouteDefinition metadataRouteDefinition =
                    new(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, Option<Action<RouteHandlerBuilder>>.None, null);

                var result = await webApiEndpointHttpContextDispatcher.HandleSuccessResponseAsync(httpContext, responseDto, metadataRouteDefinition, CancellationToken.None);

                result.ShouldBeSuccess();

                httpContext.Response.StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);

                httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
                using var streamReader = new StreamReader(httpContext.Response.Body);
                var requestBody = await streamReader.ReadToEndAsync();

                var receivedEmptyJson = JsonSerializer.Deserialize<ResponseDto>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

                receivedEmptyJson.Should().BeEquivalentTo(responseDto);
            }
        }
    }

    public class HandleFailedResponseAsync
    {
        [Fact]
        public async Task success()
        {
            const string ErrorMessage = "ERROR-MESSAGE";

            var resultError = ErrorMessage.ToResultError();

            var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Body = new MemoryStream();
            httpContext.Response.Body = new MemoryStream();

            MetadataRouteDefinition metadataRouteDefinition =
                new(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, Option<Action<RouteHandlerBuilder>>.None, null);

            var result = await webApiEndpointHttpContextDispatcher.HandleFailedResponseAsync(httpContext, resultError, metadataRouteDefinition, CancellationToken.None);

            result.ShouldBeSuccess();

            httpContext.Response.StatusCode.Should().Be(metadataRouteDefinition.FailedStatusCode);

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            using var streamReader = new StreamReader(httpContext.Response.Body);
            var requestBody = await streamReader.ReadToEndAsync();

            var resultErrorStructure = JsonSerializer.Deserialize<ResultErrorStructure>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

            resultErrorStructure.Message.Should().Be(ErrorMessage);
            resultErrorStructure.Children.Should().BeEmpty();
        }
    }
}