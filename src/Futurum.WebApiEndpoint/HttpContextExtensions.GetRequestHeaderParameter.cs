using Futurum.Core.Linq;
using Futurum.Core.Option;
using Futurum.Core.Result;

using Microsoft.Extensions.Primitives;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Extension methods for <see cref="HttpContext"/>
/// </summary>
public static partial class HttpContextExtensions
{
    /// <summary>
    /// Get <see cref="StringValues"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Headers"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<StringValues> GetRequestHeaderParameterAsStringValues(this HttpContext httpContext, string parameterName) =>
        httpContext.Request.Headers.TryGetValue(parameterName)
                   .ToResult(() => $"Unable to get Request Header Parameter - '{parameterName}'. Request Header Parameters available are '{httpContext.Request.Headers.Keys.StringJoin(",")}'");

    /// <summary>
    /// Get <see cref="string"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Headers"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<string> GetRequestHeaderFirstParameterAsString(this HttpContext httpContext, string parameterName) =>
        GetRequestHeaderParameter(httpContext, parameterName,
                                 value => value.TryFirst().ToResult(() => $"Unable to find Header Parameters with name : '{parameterName}'")
                                               .Map(parameterValue => parameterValue.ToString()));

    /// <summary>
    /// Get <see cref="int"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Headers"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<int> GetRequestHeaderFirstParameterAsInt(this HttpContext httpContext, string parameterName) =>
        GetRequestHeaderFirstParameter(httpContext, parameterName,
                                      value => value.TryParseInt(() => $"Unable to parse Request Header Parameter - '{parameterName}' to Int: '{value}'"));

    /// <summary>
    /// Get <see cref="long"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Headers"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<long> GetRequestHeaderFirstParameterAsLong(this HttpContext httpContext, string parameterName) =>
        GetRequestHeaderFirstParameter(httpContext, parameterName,
                                      value => value.TryParseLong(() => $"Unable to parse Request Header Parameter - '{parameterName}' to Long: '{value}'"));

    /// <summary>
    /// Get <see cref="DateTime"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Headers"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<DateTime> GetRequestHeaderFirstParameterAsDateTime(this HttpContext httpContext, string parameterName) =>
        GetRequestHeaderFirstParameter(httpContext, parameterName,
                                      value => value.TryParseDateTime(() => $"Unable to parse Request Header Parameter - '{parameterName}' to DateTime: '{value}'"));

    /// <summary>
    /// Get <see cref="bool"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Headers"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<bool> GetRequestHeaderFirstParameterAsBool(this HttpContext httpContext, string parameterName) =>
        GetRequestHeaderFirstParameter(httpContext, parameterName,
                                      value => value.TryParseBool(() => $"Unable to parse Request Header Parameter - '{parameterName}' to Bool: '{value}'"));

    /// <summary>
    /// Get <see cref="Guid"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Headers"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<Guid> GetRequestHeaderFirstParameterAsGuid(this HttpContext httpContext, string parameterName) =>
        GetRequestHeaderFirstParameter(httpContext, parameterName,
                                      value => value.TryParseGuid(() => $"Unable to parse Request Header Parameter - '{parameterName}' to Guid: '{value}'"));
    
    private static Option<StringValues> TryGetValue(this IHeaderDictionary source, string key) =>
        source.TryGetValue(key, out var value) ? Option<StringValues>.From(value) : Option<StringValues>.None;

    public static Result<TR> GetRequestHeaderParameter<TR>(this HttpContext httpContext, string parameterName, Func<StringValues, Result<TR>> nextResult) =>
        GetRequestHeaderParameterAsStringValues(httpContext, parameterName)
            .Then(nextResult);

    public static Result<TR> GetRequestHeaderFirstParameter<TR>(this HttpContext httpContext, string parameterName, Func<string, Result<TR>> nextResult) =>
        GetRequestHeaderFirstParameterAsString(httpContext, parameterName)
            .Then(nextResult);
}