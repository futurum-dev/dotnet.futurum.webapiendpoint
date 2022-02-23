using System.Net;
using System.Reflection;
using System.Text.Json;

using FastMember;

using Futurum.Core.Result;

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;

namespace Futurum.WebApiEndpoint.Internal;

internal static class MapFromRequestMultipartMapper<TRequestDto>
{
    private static readonly Dictionary<int, Func<TRequestDto, MultipartSection, CancellationToken, Task<Result>>> PropertiesToMap = new();

    static MapFromRequestMultipartMapper()
    {
        var typeAccessor = TypeAccessor.Create(typeof(TRequestDto));

        var propertiesWithMapFromStreamAttribute = typeof(TRequestDto).GetProperties()
                                                                      .Where(propertyInfo => propertyInfo.GetCustomAttribute<MapFromMultipartAttribute>() != null)
                                                                      .ToList();

        foreach (var propertyInfo in propertiesWithMapFromStreamAttribute)
        {
            var mapFromStreamAttribute = propertyInfo.GetCustomAttribute<MapFromMultipartAttribute>();

            if (!propertyInfo.CanWrite)
            {
                throw new InvalidOperationException($"Property '{propertyInfo.Name}' on RequestDto type : '{typeof(TRequestDto).FullName}' is readonly");
            }

            PropertiesToMap.Add(mapFromStreamAttribute.SectionPosition, GetMapper(propertyInfo, typeAccessor, mapFromStreamAttribute));
        }
    }

    private static Func<TRequestDto, MultipartSection, CancellationToken, Task<Result>> GetMapper(PropertyInfo propertyInfo, TypeAccessor typeAccessor,
                                                                                                  MapFromMultipartAttribute mapFromMultipartAttribute) =>
        mapFromMultipartAttribute.MapFromMultipart switch
        {
            MapFromMultipart.File => GetMapFromFileMapper(propertyInfo, typeAccessor, mapFromMultipartAttribute),
            MapFromMultipart.Json => GetMapFromJsonMapper(propertyInfo, typeAccessor, mapFromMultipartAttribute),
            _                     => (_, _, _) => Result.FailAsync($"Failed to MapFrom property : '{propertyInfo.Name}' using MapFrom : '{mapFromMultipartAttribute}'")
        };

    private static Func<TRequestDto, MultipartSection, CancellationToken, Task<Result>> GetMapFromFileMapper(PropertyInfo propertyInfo, TypeAccessor typeAccessor,
                                                                                                             MapFromMultipartAttribute mapFromMultipartAttribute)
    {
        Result Map(MultipartSection multipartSection, TRequestDto requestDto, CancellationToken cancellationToken)
        {
            if (multipartSection.GetContentDispositionHeader() != null)
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

        Result Execute(MultipartSection multipartSection, TRequestDto dto, CancellationToken cancellationToken)
        {
            return Map(multipartSection, dto, cancellationToken);
        }

        return (requestDto, multipartSection, cancellationToken) => Result.Try(() => Execute(multipartSection, requestDto, cancellationToken),
                                                                               () => $"Failed to Files MapMultipart for SectionPosition : '{mapFromMultipartAttribute.SectionPosition}'")
                                                                          .ToResultAsync();
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    private static Func<TRequestDto, MultipartSection, CancellationToken, Task<Result>> GetMapFromJsonMapper(PropertyInfo propertyInfo, TypeAccessor typeAccessor,
                                                                                                             MapFromMultipartAttribute mapFromMultipartAttribute)
    {
        async Task MapAsync(MultipartSection multipartSection, TRequestDto requestDto, CancellationToken cancellationToken)
        {
            multipartSection.Body.Seek(0, SeekOrigin.Begin);
            var value = await JsonSerializer.DeserializeAsync(multipartSection.Body, propertyInfo.PropertyType, JsonSerializerOptions, cancellationToken);
            typeAccessor[requestDto, propertyInfo.Name] = value;
        }

        Task Execute(MultipartSection multipartSection, TRequestDto dto, CancellationToken cancellationToken)
        {
            return MapAsync(multipartSection, dto, cancellationToken);
        }

        return (requestDto, multipartSection, cancellationToken) => Result.TryAsync(() => Execute(multipartSection, requestDto, cancellationToken),
                                                                                    () => $"Failed to Json MapMultipart for SectionPosition : '{mapFromMultipartAttribute.SectionPosition}'");
    }

    private static Task<Result> MapMultipartAsync(HttpContext httpContext, TRequestDto dto, CancellationToken cancellationToken)
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
                    var result = await mapper(dto, section, cancellationToken);

                    results.Add(result);
                }

                section = await reader.ReadNextSectionAsync(cancellationToken);
                sectionCount++;
            }

            return results.Combine();
        }

        return Result.TryAsync(Execute, () => $"Failed to MapMultipart");
    }

    public static Task<Result<TRequestDto>> MapAsync(HttpContext httpContext, TRequestDto dto, CancellationToken cancellationToken) =>
        PropertiesToMap.Count switch
        {
            0 => dto.ToResultOkAsync(),
            _ => MapMultipartAsync(httpContext, dto, cancellationToken).MapAsync(() => dto)
        };
}