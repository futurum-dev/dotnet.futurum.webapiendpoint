using System.Net;
using System.Reflection;
using System.Text.Json;

using FastMember;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Futurum.WebApiEndpoint.Internal;

internal static class MapFromRequestMultipartMapper<TRequestDto>
{
    private static readonly Dictionary<int, Func<TRequestDto, JsonSerializerOptions, MultipartSection, CancellationToken, Task<Result>>> PropertiesToMap = new();

    static MapFromRequestMultipartMapper()
    {
        var typeAccessor = TypeAccessor.Create(typeof(TRequestDto));

        var propertiesWithMapFromMultipartAttribute = WebApiEndpointMetadataTypeService.GetMapFromMultipartProperties(typeof(TRequestDto));

        foreach (var (propertyInfo, mapFromMultipartAttribute) in propertiesWithMapFromMultipartAttribute)
        {
            if (!propertyInfo.CanWrite)
            {
                throw new InvalidOperationException($"Property '{propertyInfo.Name}' on RequestDto type : '{typeof(TRequestDto).FullName}' is readonly");
            }

            PropertiesToMap.Add(mapFromMultipartAttribute.SectionPosition, GetMapper(propertyInfo, typeAccessor, mapFromMultipartAttribute));
        }
    }

    private static Func<TRequestDto, JsonSerializerOptions, MultipartSection, CancellationToken, Task<Result>> GetMapper(PropertyInfo propertyInfo, TypeAccessor typeAccessor,
                                                                                                                         MapFromMultipartAttribute mapFromMultipartAttribute) =>
        mapFromMultipartAttribute.MapFromMultipart switch
        {
            MapFromMultipart.File => GetMapFromFileMapper(propertyInfo, typeAccessor, mapFromMultipartAttribute),
            MapFromMultipart.Json => GetMapFromJsonMapper(propertyInfo, typeAccessor, mapFromMultipartAttribute),
            _                     => (_, _, _, _) => Result.FailAsync($"Failed to MapFrom property : '{propertyInfo.Name}' using MapFrom : '{mapFromMultipartAttribute}'")
        };

    private static Func<TRequestDto, JsonSerializerOptions, MultipartSection, CancellationToken, Task<Result>> GetMapFromFileMapper(PropertyInfo propertyInfo, TypeAccessor typeAccessor,
                                                                                                                                    MapFromMultipartAttribute mapFromMultipartAttribute)
    {
        Result Execute(MultipartSection multipartSection, TRequestDto requestDto)
        {
            if (propertyInfo.PropertyType != typeof(IFormFile) && propertyInfo.PropertyType != typeof(FormFile))
            {
                return Result.Fail($"Property '{propertyInfo.Name}' (type : '{propertyInfo.PropertyType.FullName}') on RequestDto type : '{typeof(TRequestDto).FullName}' is not type : '{typeof(IFormFile).FullName}' or '{typeof(FormFile).FullName}'");
            }
            
            if (multipartSection.GetContentDispositionHeader() != null && multipartSection.GetContentDispositionHeader()?.IsFileDisposition() is true)
            {
                var fileSection = multipartSection.AsFileSection();

                if (fileSection != null)
                {
                    // Don't trust the file name sent by the client. To display the file name, HTML-encode the value.
                    var trustedFileNameForDisplay = WebUtility.HtmlEncode(fileSection.FileName);

                    typeAccessor[requestDto, propertyInfo.Name] = new FormFile(fileSection.FileStream, 0, fileSection.FileStream.Length, fileSection.Name, trustedFileNameForDisplay);

                    return Result.Ok();
                }
            }

            return Result.Fail($"MultipartSection not set to File");
        }

        return (requestDto, _, multipartSection, _) => Result.Try(() => Execute(multipartSection, requestDto),
                                                                               () => $"Failed to Files MapMultipart for SectionPosition : '{mapFromMultipartAttribute.SectionPosition}' to Property '{propertyInfo.Name}' on RequestDto type : '{typeof(TRequestDto).FullName}'")
                                                                          .ToResultAsync();
    }

    private static Func<TRequestDto, JsonSerializerOptions, MultipartSection, CancellationToken, Task<Result>> GetMapFromJsonMapper(PropertyInfo propertyInfo, TypeAccessor typeAccessor,
                                                                                                                                    MapFromMultipartAttribute mapFromMultipartAttribute)
    {
        async Task Execute(MultipartSection multipartSection, TRequestDto requestDto, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken)
        {
            multipartSection.Body.Seek(0, SeekOrigin.Begin);
            var value = await JsonSerializer.DeserializeAsync(multipartSection.Body, propertyInfo.PropertyType, jsonSerializerOptions, cancellationToken);
            typeAccessor[requestDto, propertyInfo.Name] = value;
        }

        return (requestDto, jsonSerializerOptions, multipartSection, cancellationToken) => Result.TryAsync(() => Execute(multipartSection, requestDto, jsonSerializerOptions, cancellationToken),
                                                                                                           () => $"Failed to Json MapMultipart for SectionPosition : '{mapFromMultipartAttribute.SectionPosition}' to Property '{propertyInfo.Name}' on RequestDto type : '{typeof(TRequestDto).FullName}'");
    }

    private static Task<Result> MapMultipartAsync(HttpContext httpContext, JsonSerializerOptions jsonSerializerOptions, TRequestDto dto, CancellationToken cancellationToken)
    {
        async Task<Result> Execute()
        {
            var results = new List<Result>();

            var boundary = httpContext.Request.GetMultipartBoundary();

            var reader = new MultipartReader(boundary, httpContext.Request.Body);

            var sectionCount = 0;
            var section = await reader.ReadNextSectionAsync(cancellationToken);
            while (section != null)
            {
                if (PropertiesToMap.TryGetValue(sectionCount, out var mapper))
                {
                    var result = await mapper(dto, jsonSerializerOptions, section, cancellationToken);

                    results.Add(result);
                }

                section = await reader.ReadNextSectionAsync(cancellationToken);
                sectionCount++;
            }

            return results.Combine();
        }

        return Result.TryAsync(Execute, () => $"Failed to MapMultipart");
    }

    public static Task<Result> MapAsync(HttpContext httpContext, JsonSerializerOptions jsonSerializerOptions, TRequestDto dto, CancellationToken cancellationToken) =>
        PropertiesToMap.Count switch
        {
            0 => Result.OkAsync(),
            _ => MapMultipartAsync(httpContext, jsonSerializerOptions, dto, cancellationToken)
        };
}