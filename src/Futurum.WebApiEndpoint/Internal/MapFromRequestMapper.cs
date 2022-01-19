using System.Reflection;

using FastMember;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;

namespace Futurum.WebApiEndpoint.Internal;

internal class MapFromRequestMapper<TRequestDto>
    where TRequestDto : class
{
    private static readonly Func<TRequestDto, HttpContext, Result>[] PropertiesToMap;

    static MapFromRequestMapper()
    {
        var typeAccessor = TypeAccessor.Create(typeof(TRequestDto));

        var propertiesWithMapFromAttribute = typeof(TRequestDto).GetProperties()
                                                                .Where(propertyInfo => propertyInfo.GetCustomAttribute<MapFromAttribute>() != null)
                                                                .ToList();
        
        PropertiesToMap = new Func<TRequestDto, HttpContext, Result>[propertiesWithMapFromAttribute.Count];

        var count = 0;
        foreach (var propertyInfo in propertiesWithMapFromAttribute)
        {
            var mapFromAttribute = propertyInfo.GetCustomAttribute<MapFromAttribute>();

            if (!propertyInfo.CanWrite)
            {
                throw new InvalidOperationException($"Property '{propertyInfo.Name}' on RequestDto type : '{typeof(TRequestDto).FullName}' is readonly");
            }

            PropertiesToMap[count] = GetMapper(propertyInfo, typeAccessor, mapFromAttribute);

            count++;
        }
    }

    private static Func<TRequestDto, HttpContext, Result> GetMapper(PropertyInfo propertyInfo, TypeAccessor typeAccessor, MapFromAttribute mapFromAttribute) =>
        mapFromAttribute.MapFrom switch
        {
            MapFrom.Path   => GetMapFromPathMapper(propertyInfo, typeAccessor, mapFromAttribute),
            MapFrom.Query  => GetMapFromQueryMapper(propertyInfo, typeAccessor, mapFromAttribute),
            MapFrom.Header => GetMapFromHeaderMapper(propertyInfo, typeAccessor, mapFromAttribute),
            MapFrom.Cookie => GetMapFromCookieMapper(propertyInfo, typeAccessor, mapFromAttribute),
            MapFrom.Form => GetMapFromFormMapper(propertyInfo, typeAccessor, mapFromAttribute),
            _              => (_, _) => Result.Fail($"Failed to MapFrom property : '{propertyInfo.Name}' using MapFrom : '{mapFromAttribute}'")
        };

    private static Func<TRequestDto, HttpContext, Result> GetMapFromPathMapper(PropertyInfo propertyInfo, TypeAccessor typeAccessor, MapFromAttribute mapFromAttribute)
    {
        if (propertyInfo.PropertyType == typeof(string))
            return (requestDto, httpContext) => httpContext.GetRequestPathParameterAsString(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(int))
            return (requestDto, httpContext) => httpContext.GetRequestPathParameterAsInt(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(long))
            return (requestDto, httpContext) => httpContext.GetRequestPathParameterAsLong(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);

        return (_, _) => Result.Fail($"Failed to MapFrom property : '{propertyInfo.Name}', unknown PropertyType : '{propertyInfo.PropertyType}'");
    }

    private static Func<TRequestDto, HttpContext, Result> GetMapFromQueryMapper(PropertyInfo propertyInfo, TypeAccessor typeAccessor, MapFromAttribute mapFromAttribute)
    {
        if (propertyInfo.PropertyType == typeof(string))
            return (requestDto, httpContext) => httpContext.GetRequestQueryFirstParameterAsString(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(int))
            return (requestDto, httpContext) => httpContext.GetRequestQueryFirstParameterAsInt(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(long))
            return (requestDto, httpContext) => httpContext.GetRequestQueryFirstParameterAsLong(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);

        return (_, _) => Result.Fail($"Failed to MapFrom property : '{propertyInfo.Name}', unknown PropertyType : '{propertyInfo.PropertyType}'");
    }

    private static Func<TRequestDto, HttpContext, Result> GetMapFromHeaderMapper(PropertyInfo propertyInfo, TypeAccessor typeAccessor, MapFromAttribute mapFromAttribute)
    {
        if (propertyInfo.PropertyType == typeof(string))
            return (requestDto, httpContext) => httpContext.GetRequestHeaderFirstParameterAsString(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(int))
            return (requestDto, httpContext) => httpContext.GetRequestHeaderFirstParameterAsInt(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(long))
            return (requestDto, httpContext) => httpContext.GetRequestHeaderFirstParameterAsLong(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);

        return (_, _) => Result.Fail($"Failed to MapFrom property : '{propertyInfo.Name}', unknown PropertyType : '{propertyInfo.PropertyType}'");
    }

    private static Func<TRequestDto, HttpContext, Result> GetMapFromCookieMapper(PropertyInfo propertyInfo, TypeAccessor typeAccessor, MapFromAttribute mapFromAttribute)
    {
        if (propertyInfo.PropertyType == typeof(string))
            return (requestDto, httpContext) => httpContext.GetRequestCookieFirstParameterAsString(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(int))
            return (requestDto, httpContext) => httpContext.GetRequestCookieFirstParameterAsInt(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(long))
            return (requestDto, httpContext) => httpContext.GetRequestCookieFirstParameterAsLong(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);

        return (_, _) => Result.Fail($"Failed to MapFrom property : '{propertyInfo.Name}', unknown PropertyType : '{propertyInfo.PropertyType}'");
    }

    private static Func<TRequestDto, HttpContext, Result> GetMapFromFormMapper(PropertyInfo propertyInfo, TypeAccessor typeAccessor, MapFromAttribute mapFromAttribute)
    {
        if (propertyInfo.PropertyType == typeof(string))
            return (requestDto, httpContext) => httpContext.GetRequestFormFirstParameterAsString(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(int))
            return (requestDto, httpContext) => httpContext.GetRequestFormFirstParameterAsInt(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);
        if (propertyInfo.PropertyType == typeof(long))
            return (requestDto, httpContext) => httpContext.GetRequestFormFirstParameterAsLong(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value);

        return (_, _) => Result.Fail($"Failed to MapFrom property : '{propertyInfo.Name}', unknown PropertyType : '{propertyInfo.PropertyType}'");
    }

    public static Result<TRequestDto> Map(HttpContext httpContext) =>
        CreateDto().Then(dto => Map(httpContext, dto));

    public static Result<TRequestDto> Map(HttpContext httpContext, TRequestDto dto) =>
        PropertiesToMap.Length switch
        {
            0 => dto.ToResultOk(),
            1 => PropertiesToMap[0](dto, httpContext).Map(() => dto),
            _ => PropertiesToMap.FlatMapSequentialUntilFailure(x => x(dto, httpContext)).Map(() => dto)
        };

    private static Result<TRequestDto> CreateDto() =>
        typeof(TRequestDto).GetConstructor(Type.EmptyTypes) != null
            ? (Activator.CreateInstance(typeof(TRequestDto)) as TRequestDto).ToResultOk()
            : Result.Fail<TRequestDto>($"RequestDto type : '{typeof(TRequestDto).FullName}', does not have an empty constructor");
}