using Futurum.Core.Linq;
using Futurum.Core.Option;
using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Extension methods for <see cref="HttpContext"/>
/// </summary>
public static partial class HttpContextExtensions
{
    /// <summary>
    /// Get <see cref="string"/> from <see cref="HttpContext"/> <see cref="HttpRequest.RouteValues"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<string?> GetRequestPathParameterAsString(this HttpContext httpContext, string parameterName) =>
        httpContext.Request.RouteValues.TryGetValue(parameterName).Map(value => value?.ToString())
                   .ToResult(() => $"Unable to get Request Path Parameter - '{parameterName}'. Request Path Parameters available are '{httpContext.Request.RouteValues.Keys.StringJoin(",")}'");

    /// <summary>
    /// Get <see cref="int"/> from <see cref="HttpContext"/> <see cref="HttpRequest.RouteValues"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<int> GetRequestPathParameterAsInt(this HttpContext httpContext, string parameterName) =>
        GetRequestPathParameter(httpContext, parameterName,
                                value => value.TryParseInt(() => $"Unable to parse Request Path Parameter - '{parameterName}' to Int: '{value}'"));

    /// <summary>
    /// Get <see cref="long"/> from <see cref="HttpContext"/> <see cref="HttpRequest.RouteValues"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<long> GetRequestPathParameterAsLong(this HttpContext httpContext, string parameterName) =>
        GetRequestPathParameter(httpContext, parameterName,
                                value => value.TryParseLong(() => $"Unable to parse Request Path Parameter - '{parameterName}' to Long: '{value}'"));

    /// <summary>
    /// Get <see cref="DateTime"/> from <see cref="HttpContext"/> <see cref="HttpRequest.RouteValues"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<DateTime> GetRequestPathParameterAsDateTime(this HttpContext httpContext, string parameterName) =>
        GetRequestPathParameter(httpContext, parameterName,
                                value => value.TryParseDateTime(() => $"Unable to parse Request Path Parameter - '{parameterName}' to DateTime: '{value}'"));

    /// <summary>
    /// Get <see cref="bool"/> from <see cref="HttpContext"/> <see cref="HttpRequest.RouteValues"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<bool> GetRequestPathParameterAsBool(this HttpContext httpContext, string parameterName) =>
        GetRequestPathParameter(httpContext, parameterName,
                                value => value.TryParseBool(() => $"Unable to parse Request Path Parameter - '{parameterName}' to Bool: '{value}'"));

    /// <summary>
    /// Get <see cref="Guid"/> from <see cref="HttpContext"/> <see cref="HttpRequest.RouteValues"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<Guid> GetRequestPathParameterAsGuid(this HttpContext httpContext, string parameterName) =>
        GetRequestPathParameter(httpContext, parameterName,
                                value => value.TryParseGuid(() => $"Unable to parse Request Path Parameter - '{parameterName}' to Guid: '{value}'"));

    public static Result<TR> GetRequestPathParameter<TR>(this HttpContext httpContext, string parameterName, Func<string?, Result<TR>> nextResult) =>
        GetRequestPathParameterAsString(httpContext, parameterName)
            .Then(nextResult);
}