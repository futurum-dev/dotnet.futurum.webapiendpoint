using System.Reflection;

using FastMember;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Internal;

internal static class MapFromRequestMapper<TRequestDto>
{
    private static readonly Func<TRequestDto, HttpContext, CancellationToken, Result>[] PropertiesToMap;

    static MapFromRequestMapper()
    {
        var typeAccessor = TypeAccessor.Create(typeof(TRequestDto));

        var propertiesWithMapFromAttribute = WebApiEndpointMetadataTypeService.GetMapFromProperties(typeof(TRequestDto));

        PropertiesToMap = new Func<TRequestDto, HttpContext, CancellationToken, Result>[propertiesWithMapFromAttribute.Count];

        var count = 0;
        foreach (var (propertyInfo, mapFromAttribute) in propertiesWithMapFromAttribute)
        {
            if (!propertyInfo.CanWrite)
            {
                throw new InvalidOperationException($"Property '{propertyInfo.Name}' on RequestDto type : '{typeof(TRequestDto).FullName}' is readonly");
            }

            PropertiesToMap[count] = GetMapper(propertyInfo, typeAccessor, mapFromAttribute);

            count++;
        }
    }

    private static Func<TRequestDto, HttpContext, CancellationToken, Result> GetMapper(PropertyInfo propertyInfo, TypeAccessor typeAccessor, MapFromAttribute mapFromAttribute) =>
        mapFromAttribute.MapFrom switch
        {
            MapFrom.Path   => GetMapFromPathMapper(propertyInfo, typeAccessor, mapFromAttribute),
            MapFrom.Query  => GetMapFromQueryMapper(propertyInfo, typeAccessor, mapFromAttribute),
            MapFrom.Header => GetMapFromHeaderMapper(propertyInfo, typeAccessor, mapFromAttribute),
            MapFrom.Cookie => GetMapFromCookieMapper(propertyInfo, typeAccessor, mapFromAttribute),
            _              => (_, _, _) => Result.Fail($"Failed to MapFrom property : '{propertyInfo.Name}' using MapFrom : '{mapFromAttribute}'")
        };

    private static Func<TRequestDto, HttpContext, CancellationToken, Result> GetMapFromPathMapper(PropertyInfo propertyInfo, TypeAccessor typeAccessor, MapFromAttribute mapFromAttribute)
    {
        if (propertyInfo.PropertyType == typeof(string))
            return (requestDto, httpContext, _) => httpContext.GetRequestPathParameterAsString(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(int))
            return (requestDto, httpContext, _) => httpContext.GetRequestPathParameterAsInt(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(long))
            return (requestDto, httpContext, _) => httpContext.GetRequestPathParameterAsLong(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(DateTime))
            return (requestDto, httpContext, _) => httpContext.GetRequestPathParameterAsDateTime(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);

        return (_, _, _) => Result.Fail($"Failed to MapFromPath property : '{propertyInfo.Name}', unknown PropertyType : '{propertyInfo.PropertyType}'");
    }

    private static Func<TRequestDto, HttpContext, CancellationToken, Result> GetMapFromQueryMapper(PropertyInfo propertyInfo, TypeAccessor typeAccessor, MapFromAttribute mapFromAttribute)
    {
        if (propertyInfo.PropertyType == typeof(string))
            return (requestDto, httpContext, _) => httpContext.GetRequestQueryFirstParameterAsString(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(int))
            return (requestDto, httpContext, _) => httpContext.GetRequestQueryFirstParameterAsInt(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(long))
            return (requestDto, httpContext, _) => httpContext.GetRequestQueryFirstParameterAsLong(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(DateTime))
            return (requestDto, httpContext, _) => httpContext.GetRequestQueryFirstParameterAsDateTime(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);

        return (_, _, _) => Result.Fail($"Failed to MapFromQuery property : '{propertyInfo.Name}', unknown PropertyType : '{propertyInfo.PropertyType}'");
    }

    private static Func<TRequestDto, HttpContext, CancellationToken, Result> GetMapFromHeaderMapper(PropertyInfo propertyInfo, TypeAccessor typeAccessor, MapFromAttribute mapFromAttribute)
    {
        if (propertyInfo.PropertyType == typeof(string))
            return (requestDto, httpContext, _) => httpContext.GetRequestHeaderFirstParameterAsString(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(int))
            return (requestDto, httpContext, _) => httpContext.GetRequestHeaderFirstParameterAsInt(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(long))
            return (requestDto, httpContext, _) => httpContext.GetRequestHeaderFirstParameterAsLong(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(DateTime))
            return (requestDto, httpContext, _) => httpContext.GetRequestHeaderFirstParameterAsDateTime(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);

        return (_, _, _) => Result.Fail($"Failed to MapFromHeader property : '{propertyInfo.Name}', unknown PropertyType : '{propertyInfo.PropertyType}'");
    }

    private static Func<TRequestDto, HttpContext, CancellationToken, Result> GetMapFromCookieMapper(PropertyInfo propertyInfo, TypeAccessor typeAccessor, MapFromAttribute mapFromAttribute)
    {
        if (propertyInfo.PropertyType == typeof(string))
            return (requestDto, httpContext, _) => httpContext.GetRequestCookieFirstParameterAsString(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(int))
            return (requestDto, httpContext, _) => httpContext.GetRequestCookieFirstParameterAsInt(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(long))
            return (requestDto, httpContext, _) => httpContext.GetRequestCookieFirstParameterAsLong(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(DateTime))
            return (requestDto, httpContext, _) => httpContext.GetRequestCookieFirstParameterAsDateTime(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);

        return (_, _, _) => Result.Fail($"Failed to MapFromCookie property : '{propertyInfo.Name}', unknown PropertyType : '{propertyInfo.PropertyType}'");
    }

    public static Result<TRequestDto> Map(HttpContext httpContext, TRequestDto dto, CancellationToken cancellationToken) =>
        PropertiesToMap.Length switch
        {
            0 => dto.ToResultOk(),
            1 => PropertiesToMap[0](dto, httpContext, cancellationToken).Map(() => dto),
            _ => PropertiesToMap.FlatMapSequentialUntilFailure(func => func(dto, httpContext, cancellationToken)).Map(() => dto)
        };
}