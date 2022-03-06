using System.Net.Mime;
using System.Text;
using System.Text.Json;

using Futurum.Test.Result;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class RequestJsonReaderTests
{
    public class OnlyJson
    {
        public record RequestDto(string FirstName, int Age);

        [Fact]
        public async Task success()
        {
            var firstName = Guid.NewGuid().ToString();
            var age = 10;

            var requestDto = new RequestDto(firstName, age);

            var requestJsonReader = new RequestJsonReader<RequestDto>(Options.Create(new JsonOptions()));

            var httpContext = new DefaultHttpContext();

            var json = JsonSerializer.Serialize(requestDto);
            var stream = new MemoryStream(Encoding.Default.GetBytes(json));
            httpContext.Request.Body = stream;
            httpContext.Request.ContentLength = stream.Length;
            httpContext.Request.ContentType = MediaTypeNames.Application.Json;

            var metadataDefinition = new MetadataDefinition(null,null, null, null);
            
            var result = await requestJsonReader.ExecuteAsync(httpContext, metadataDefinition, CancellationToken.None);

            result.ShouldBeSuccessWithValue(requestDto);
        }

        [Fact]
        public async Task failure()
        {
            var firstName = Guid.NewGuid().ToString();
            var age = 10;

            var requestDto = new RequestDto(firstName, age);

            var requestJsonReader = new RequestJsonReader<FailingDeserializeObject>(Options.Create(new JsonOptions()));

            var httpContext = new DefaultHttpContext();

            var json = JsonSerializer.Serialize(requestDto);
            var stream = new MemoryStream(Encoding.Default.GetBytes(json));
            httpContext.Request.Body = stream;
            httpContext.Request.ContentLength = stream.Length;
            httpContext.Request.ContentType = MediaTypeNames.Application.Json;

            var metadataDefinition = new MetadataDefinition(null,null, null, null);

            var result = await requestJsonReader.ExecuteAsync(httpContext, metadataDefinition, CancellationToken.None);

            result.ShouldBeFailureWithErrorContaining("Failed to deserialize request as json");
        }

        public record FailingDeserializeObject(int FirstName);
    }

    public class OnlyMapFrom
    {
        public record RequestDto
        {
            [MapFromPath("Id")] public string Id { get; set; }
        }

        [Fact]
        public async Task success()
        {
            var value = Guid.NewGuid().ToString();

            var requestJsonReader = new RequestJsonReader<RequestDto>(Options.Create(new JsonOptions()));

            var metadataMapFromParameterDefinition = new MetadataMapFromParameterDefinition(nameof(RequestDto.Id), typeof(RequestDto).GetProperty(nameof(RequestDto.Id)), new MapFromPathAttribute(nameof(RequestDto.Id)));
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition> { metadataMapFromParameterDefinition });

            var metadataDefinition = new MetadataDefinition(null, null, metadataMapFromDefinition, null);
            
            var httpContext = new DefaultHttpContext();
            httpContext.Request.RouteValues = new RouteValueDictionary(new Dictionary<string, string> { { "Id", value } });

            var result = await requestJsonReader.ExecuteAsync(httpContext, metadataDefinition, CancellationToken.None);

            result.ShouldBeSuccessWithValue(new RequestDto { Id = value });
        }
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

            var requestJsonReader = new RequestJsonReader<RequestDto>(Options.Create(new JsonOptions()));
            
            var metadataMapFromParameterDefinition = new MetadataMapFromParameterDefinition(nameof(RequestDto.Id), typeof(RequestDto).GetProperty(nameof(RequestDto.Id)), new MapFromPathAttribute(nameof(RequestDto.Id)));
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition> { metadataMapFromParameterDefinition });

            var metadataDefinition = new MetadataDefinition(null, null, metadataMapFromDefinition, null);

            var httpContext = new DefaultHttpContext();

            httpContext.Request.RouteValues = new RouteValueDictionary(new Dictionary<string, string> { { "Id", idValue } });

            var json = JsonSerializer.Serialize(requestDto);
            var stream = new MemoryStream(Encoding.Default.GetBytes(json));
            httpContext.Request.Body = stream;
            httpContext.Request.ContentLength = stream.Length;
            httpContext.Request.ContentType = MediaTypeNames.Application.Json;

            var result = await requestJsonReader.ExecuteAsync(httpContext, metadataDefinition, CancellationToken.None);

            result.ShouldBeSuccessWithValue(new RequestDto(firstName, age) { Id = idValue });
        }
    }
}