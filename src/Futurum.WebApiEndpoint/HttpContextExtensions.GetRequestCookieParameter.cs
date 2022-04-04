using Futurum.Core.Linq;
using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Extension methods for <see cref="HttpContext"/>
/// </summary>
public static partial class HttpContextExtensions
{
    /// <summary>
    /// Get <see cref="string"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Cookies"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<string> GetRequestCookieFirstParameterAsString(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Cookies.TryGetValue(parameterName, out var value))
        {
            return !string.IsNullOrEmpty(value)
                ? Result.Ok(value)
                : Result.Fail<string>($"Unable to parse Request Cookie Parameter - '{parameterName}' to String: '{value}'");
        }

        return Result.Fail<string>($"Unable to get Request Cookie Parameter - '{parameterName}'. Request Cookie Parameters available are '{httpContext.Request.Cookies.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="int"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Cookies"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<int> GetRequestCookieFirstParameterAsInt(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Cookies.TryGetValue(parameterName, out var value))
        {
            return int.TryParse(value, out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<int>($"Unable to parse Request Cookie Parameter - '{parameterName}' to Int: '{value}'");
        }

        return Result.Fail<int>($"Unable to get Request Cookie Parameter - '{parameterName}'. Request Cookie Parameters available are '{httpContext.Request.Cookies.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="long"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Cookies"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<long> GetRequestCookieFirstParameterAsLong(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Cookies.TryGetValue(parameterName, out var value))
        {
            return long.TryParse(value, out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<long>($"Unable to parse Request Cookie Parameter - '{parameterName}' to Long: '{value}'");
        }

        return Result.Fail<long>($"Unable to get Request Cookie Parameter - '{parameterName}'. Request Cookie Parameters available are '{httpContext.Request.Cookies.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="DateTime"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Cookies"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<DateTime> GetRequestCookieFirstParameterAsDateTime(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Cookies.TryGetValue(parameterName, out var value))
        {
            return DateTime.TryParse(value, out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<DateTime>($"Unable to parse Request Cookie Parameter - '{parameterName}' to DateTime: '{value}'");
        }

        return Result.Fail<DateTime>($"Unable to get Request Cookie Parameter - '{parameterName}'. Request Cookie Parameters available are '{httpContext.Request.Cookies.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="bool"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Cookies"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<bool> GetRequestCookieFirstParameterAsBool(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Cookies.TryGetValue(parameterName, out var value))
        {
            return bool.TryParse(value, out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<bool>($"Unable to parse Request Cookie Parameter - '{parameterName}' to Bool: '{value}'");
        }

        return Result.Fail<bool>($"Unable to get Request Cookie Parameter - '{parameterName}'. Request Cookie Parameters available are '{httpContext.Request.Cookies.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="Guid"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Cookies"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<Guid> GetRequestCookieFirstParameterAsGuid(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Cookies.TryGetValue(parameterName, out var value))
        {
            return Guid.TryParse(value, out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<Guid>($"Unable to parse Request Cookie Parameter - '{parameterName}' to Guid: '{value}'");
        }

        return Result.Fail<Guid>($"Unable to get Request Cookie Parameter - '{parameterName}'. Request Cookie Parameters available are '{httpContext.Request.Cookies.Keys.StringJoin(",")}'");
    }
}