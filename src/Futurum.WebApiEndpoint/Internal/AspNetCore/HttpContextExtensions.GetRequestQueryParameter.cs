using Futurum.Core.Linq;
using Futurum.Core.Option;
using Futurum.Core.Result;

using Microsoft.Extensions.Primitives;

namespace Futurum.WebApiEndpoint.Internal.AspNetCore;

/// <summary>
/// Extension methods for <see cref="HttpContext"/>
/// </summary>
public static partial class HttpContextExtensions
{
    /// <summary>
    /// Get <see cref="StringValues"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Query"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<StringValues> GetRequestQueryParameterAsStringValues(this HttpContext httpContext, string parameterName) =>
        httpContext.Request.Query.TryGetValue(parameterName)
                   .ToResult(() => $"Unable to get Request Query Parameter - '{parameterName}'. Request Query Parameters available are '{httpContext.Request.Query.Keys.StringJoin(",")}'");

    /// <summary>
    /// Get <see cref="string"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Query"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<string> GetRequestQueryFirstParameterAsString(this HttpContext httpContext, string parameterName) =>
        GetRequestQueryParameter(httpContext, parameterName,
                                 value => value.TryFirst().ToResult(() => $"Unable to find Query Parameters with name : '{parameterName}'")
                                               .Map(parameterValue => parameterValue.ToString()));

    /// <summary>
    /// Get <see cref="int"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Query"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<int> GetRequestQueryFirstParameterAsInt(this HttpContext httpContext, string parameterName) =>
        GetRequestQueryFirstParameter(httpContext, parameterName,
                                      value => value.TryParseInt(() => $"Unable to parse Request Query Parameter - '{parameterName}' to Int: '{value}'"));

    /// <summary>
    /// Get <see cref="long"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Query"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<long> GetRequestQueryFirstParameterAsLong(this HttpContext httpContext, string parameterName) =>
        GetRequestQueryFirstParameter(httpContext, parameterName,
                                      value => value.TryParseLong(() => $"Unable to parse Request Query Parameter - '{parameterName}' to Long: '{value}'"));
    
    private static Option<StringValues> TryGetValue(this IQueryCollection source, string key) =>
        source.TryGetValue(key, out var value) ? Option<StringValues>.From(value) : Option<StringValues>.None;

    private static Result<TR> GetRequestQueryParameter<TR>(this HttpContext httpContext, string parameterName, Func<StringValues, Result<TR>> nextResult) =>
        GetRequestQueryParameterAsStringValues(httpContext, parameterName)
            .Then(nextResult);

    private static Result<TR> GetRequestQueryFirstParameter<TR>(this HttpContext httpContext, string parameterName, Func<string, Result<TR>> nextResult) =>
        GetRequestQueryFirstParameterAsString(httpContext, parameterName)
            .Then(nextResult);
}