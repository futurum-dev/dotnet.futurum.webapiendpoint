using System.Reflection;

using FastMember;

using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Internal;

internal static class MapFromRequestMapper<TRequestDto>
{
    private static readonly Func<TRequestDto, HttpContext, CancellationToken, Result>[] PropertiesToMap;

    private static readonly Dictionary<Type, Func<PropertyInfo, TypeAccessor, MapFromAttribute, Func<TRequestDto, HttpContext, CancellationToken, Result>>> PathMapper = new()
    {
        {
            typeof(string),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestPathParameterAsString(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(int),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestPathParameterAsInt(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(long),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestPathParameterAsLong(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(DateTime),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestPathParameterAsDateTime(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(bool),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestPathParameterAsBool(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(Guid),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestPathParameterAsGuid(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
    };

    private static readonly Dictionary<Type, Func<PropertyInfo, TypeAccessor, MapFromAttribute, Func<TRequestDto, HttpContext, CancellationToken, Result>>> QueryMapper = new()
    {
        {
            typeof(string),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestQueryFirstParameterAsString(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(int),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestQueryFirstParameterAsInt(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(long),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestQueryFirstParameterAsLong(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(DateTime),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestQueryFirstParameterAsDateTime(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(bool),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestQueryFirstParameterAsBool(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(Guid),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestQueryFirstParameterAsGuid(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
    };

    private static readonly Dictionary<Type, Func<PropertyInfo, TypeAccessor, MapFromAttribute, Func<TRequestDto, HttpContext, CancellationToken, Result>>> HeaderMapper = new()
    {
        {
            typeof(string),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestHeaderFirstParameterAsString(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(int),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestHeaderFirstParameterAsInt(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(long),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestHeaderFirstParameterAsLong(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(DateTime),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestHeaderFirstParameterAsDateTime(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(bool),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestHeaderFirstParameterAsBool(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(Guid),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestHeaderFirstParameterAsGuid(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(WebApiEndpoint.Range),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
            {
                return RangeHeaderMapper.Map(httpContext)
                                        .Switch(value =>
                                                {
                                                    typeAccessor[requestDto, propertyInfo.Name] = value;
                                                    return Result.Ok();
                                                },
                                                () => Result.Fail($"Failed to MapFromHeader for {typeof(WebApiEndpoint.Range).FullName} for property : '{propertyInfo.Name}'"));
            }
        },
        {
            typeof(Option<WebApiEndpoint.Range>),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
            {
                RangeHeaderMapper.Map(httpContext)
                                 .DoSwitch(value => typeAccessor[requestDto, propertyInfo.Name] = value.ToOption(),
                                           Function.DoNothing);

                return Result.Ok();
            }
        },
    };

    private static readonly Dictionary<Type, Func<PropertyInfo, TypeAccessor, MapFromAttribute, Func<TRequestDto, HttpContext, CancellationToken, Result>>> CookieMapper = new()
    {
        {
            typeof(string),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestCookieFirstParameterAsString(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(int),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestCookieFirstParameterAsInt(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(long),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestCookieFirstParameterAsLong(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(DateTime),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestCookieFirstParameterAsDateTime(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(bool),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestCookieFirstParameterAsBool(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
        {
            typeof(Guid),
            (propertyInfo, typeAccessor, mapFromAttribute) => (requestDto, httpContext, _) =>
                httpContext.GetRequestCookieFirstParameterAsGuid(mapFromAttribute.Name).Do(value => typeAccessor[requestDto, propertyInfo.Name] = value)
        },
    };

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
            MapFrom.Path => PathMapper.TryGetValue(propertyInfo.PropertyType, out var mapper)
                ? mapper(propertyInfo, typeAccessor, mapFromAttribute)
                : (_, _, _) => Result.Fail($"Failed to MapFromPath property : '{propertyInfo.Name}', unknown PropertyType : '{propertyInfo.PropertyType}'"),
            MapFrom.Query => QueryMapper.TryGetValue(propertyInfo.PropertyType, out var mapper)
                ? mapper(propertyInfo, typeAccessor, mapFromAttribute)
                : (_, _, _) => Result.Fail($"Failed to MapFromQuery property : '{propertyInfo.Name}', unknown PropertyType : '{propertyInfo.PropertyType}'"),
            MapFrom.Header => HeaderMapper.TryGetValue(propertyInfo.PropertyType, out var mapper)
                ? mapper(propertyInfo, typeAccessor, mapFromAttribute)
                : (_, _, _) => Result.Fail($"Failed to MapFromHeader property : '{propertyInfo.Name}', unknown PropertyType : '{propertyInfo.PropertyType}'"),
            MapFrom.Cookie => CookieMapper.TryGetValue(propertyInfo.PropertyType, out var mapper)
                ? mapper(propertyInfo, typeAccessor, mapFromAttribute)
                : (_, _, _) => Result.Fail($"Failed to MapFromCookie property : '{propertyInfo.Name}', unknown PropertyType : '{propertyInfo.PropertyType}'"),
            _ => (_, _, _) => Result.Fail($"Failed to MapFrom property : '{propertyInfo.Name}' using MapFrom : '{mapFromAttribute}'")
        };

    public static Result Map(HttpContext httpContext, TRequestDto dto, CancellationToken cancellationToken) =>
        PropertiesToMap.Length switch
        {
            0 => Result.Ok(),
            1 => PropertiesToMap[0](dto, httpContext, cancellationToken),
            _ => PropertiesToMap.FlatMapSequentialUntilFailure(func => func(dto, httpContext, cancellationToken))
        };
}