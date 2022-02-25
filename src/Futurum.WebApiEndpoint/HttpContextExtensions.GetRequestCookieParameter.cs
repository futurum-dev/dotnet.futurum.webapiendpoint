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
    /// Get <see cref="StringValues"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Cookies"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<StringValues> GetRequestCookieParameterAsStringValues(this HttpContext httpContext, string parameterName) =>
        httpContext.Request.Cookies.TryGetValue(parameterName)
                   .ToResult(() => $"Unable to get Request Cookie Parameter - '{parameterName}'. Request Cookie Parameters available are '{httpContext.Request.Cookies.Keys.StringJoin(",")}'");

    /// <summary>
    /// Get <see cref="string"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Cookies"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<string> GetRequestCookieFirstParameterAsString(this HttpContext httpContext, string parameterName) =>
        GetRequestCookieParameter(httpContext, parameterName,
                                 value => value.TryFirst().ToResult(() => $"Unable to find Cookie Parameters with name : '{parameterName}'")
                                               .Map(parameterValue => parameterValue.ToString()));

    /// <summary>
    /// Get <see cref="int"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Cookies"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<int> GetRequestCookieFirstParameterAsInt(this HttpContext httpContext, string parameterName) =>
        GetRequestCookieFirstParameter(httpContext, parameterName,
                                      value => value.TryParseInt(() => $"Unable to parse Request Cookie Parameter - '{parameterName}' to Int: '{value}'"));

    /// <summary>
    /// Get <see cref="long"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Cookies"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<long> GetRequestCookieFirstParameterAsLong(this HttpContext httpContext, string parameterName) =>
        GetRequestCookieFirstParameter(httpContext, parameterName,
                                      value => value.TryParseLong(() => $"Unable to parse Request Cookie Parameter - '{parameterName}' to Long: '{value}'"));

    /// <summary>
    /// Get <see cref="DateTime"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Cookies"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<DateTime> GetRequestCookieFirstParameterAsDateTime(this HttpContext httpContext, string parameterName) =>
        GetRequestCookieFirstParameter(httpContext, parameterName,
                                      value => value.TryParseDateTime(() => $"Unable to parse Request Cookie Parameter - '{parameterName}' to DateTime: '{value}'"));
    
    private static Option<StringValues> TryGetValue(this IRequestCookieCollection source, string key) =>
        source.TryGetValue(key, out var value) ? Option<StringValues>.From(value) : Option<StringValues>.None;
    
    public static Result<TR> GetRequestCookieParameter<TR>(this HttpContext httpContext, string parameterName, Func<StringValues, Result<TR>> nextResult) =>
        GetRequestCookieParameterAsStringValues(httpContext, parameterName)
            .Then(nextResult);

    public static Result<TR> GetRequestCookieFirstParameter<TR>(this HttpContext httpContext, string parameterName, Func<string, Result<TR>> nextResult) =>
        GetRequestCookieFirstParameterAsString(httpContext, parameterName)
            .Then(nextResult);
}