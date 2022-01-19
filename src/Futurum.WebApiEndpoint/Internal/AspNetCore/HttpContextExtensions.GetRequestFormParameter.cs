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
    /// Get <see cref="StringValues"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Form"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<StringValues> GetRequestFormParameterAsStringValues(this HttpContext httpContext, string parameterName) =>
        httpContext.Request.Form.TryGetValue(parameterName)
                   .ToResult(() => $"Unable to get Request Form Parameter - '{parameterName}'. Request Form Parameters available are '{httpContext.Request.Form.Keys.StringJoin(",")}'");

    /// <summary>
    /// Get <see cref="string"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Form"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<string> GetRequestFormFirstParameterAsString(this HttpContext httpContext, string parameterName) =>
        GetRequestFormParameter(httpContext, parameterName,
                                 value => value.TryFirst().ToResult(() => $"Unable to find Form Parameters with name : '{parameterName}'")
                                               .Map(parameterValue => parameterValue.ToString()));

    /// <summary>
    /// Get <see cref="int"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Form"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<int> GetRequestFormFirstParameterAsInt(this HttpContext httpContext, string parameterName) =>
        GetRequestFormFirstParameter(httpContext, parameterName,
                                      value => value.TryParseInt(() => $"Unable to parse Request Form Parameter - '{parameterName}' to Int: '{value}'"));

    /// <summary>
    /// Get <see cref="long"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Form"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<long> GetRequestFormFirstParameterAsLong(this HttpContext httpContext, string parameterName) =>
        GetRequestFormFirstParameter(httpContext, parameterName,
                                      value => value.TryParseLong(() => $"Unable to parse Request Form Parameter - '{parameterName}' to Long: '{value}'"));
    
    private static Option<StringValues> TryGetValue(this IFormCollection source, string key) =>
        source.TryGetValue(key, out var value) ? Option<StringValues>.From(value) : Option<StringValues>.None;

    private static Result<TR> GetRequestFormParameter<TR>(this HttpContext httpContext, string parameterName, Func<StringValues, Result<TR>> nextResult) =>
        GetRequestFormParameterAsStringValues(httpContext, parameterName)
            .Then(nextResult);

    private static Result<TR> GetRequestFormFirstParameter<TR>(this HttpContext httpContext, string parameterName, Func<string, Result<TR>> nextResult) =>
        GetRequestFormFirstParameterAsString(httpContext, parameterName)
            .Then(nextResult);
}