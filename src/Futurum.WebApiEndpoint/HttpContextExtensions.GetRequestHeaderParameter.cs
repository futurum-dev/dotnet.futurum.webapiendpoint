using Futurum.Core.Linq;
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
    public static Result<StringValues> GetRequestHeaderParameterAsStringValues(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Headers.TryGetValue(parameterName, out var value))
        {
            return Result.Ok(value);
        }

        return Result.Fail<StringValues>($"Unable to get Request Header Parameter - '{parameterName}'. Request Header Parameters available are '{httpContext.Request.Headers.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="string"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Headers"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<string> GetRequestHeaderFirstParameterAsString(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Headers.TryGetValue(parameterName, out var value))
        {
            var firstValue = value.First();
            return !string.IsNullOrEmpty(firstValue)
                ? Result.Ok(firstValue)
                : Result.Fail<string>($"Unable to parse Request Header Parameter - '{parameterName}' to String: '{value}'");
        }

        return Result.Fail<string>($"Unable to get Request Header Parameter - '{parameterName}'. Request Header Parameters available are '{httpContext.Request.Headers.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="int"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Headers"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<int> GetRequestHeaderFirstParameterAsInt(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Headers.TryGetValue(parameterName, out var value))
        {
            return int.TryParse(value.First(), out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<int>($"Unable to parse Request Header Parameter - '{parameterName}' to Int: '{value}'");
        }

        return Result.Fail<int>($"Unable to get Request Header Parameter - '{parameterName}'. Request Header Parameters available are '{httpContext.Request.Headers.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="long"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Headers"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<long> GetRequestHeaderFirstParameterAsLong(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Headers.TryGetValue(parameterName, out var value))
        {
            return long.TryParse(value.First(), out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<long>($"Unable to parse Request Header Parameter - '{parameterName}' to Long: '{value}'");
        }

        return Result.Fail<long>($"Unable to get Request Header Parameter - '{parameterName}'. Request Header Parameters available are '{httpContext.Request.Headers.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="DateTime"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Headers"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<DateTime> GetRequestHeaderFirstParameterAsDateTime(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Headers.TryGetValue(parameterName, out var value))
        {
            return DateTime.TryParse(value.First(), out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<DateTime>($"Unable to parse Request Header Parameter - '{parameterName}' to DateTime: '{value}'");
        }

        return Result.Fail<DateTime>($"Unable to get Request Header Parameter - '{parameterName}'. Request Header Parameters available are '{httpContext.Request.Headers.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="bool"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Headers"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<bool> GetRequestHeaderFirstParameterAsBool(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Headers.TryGetValue(parameterName, out var value))
        {
            return bool.TryParse(value.First(), out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<bool>($"Unable to parse Request Header Parameter - '{parameterName}' to Bool: '{value}'");
        }

        return Result.Fail<bool>($"Unable to get Request Header Parameter - '{parameterName}'. Request Header Parameters available are '{httpContext.Request.Headers.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="Guid"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Headers"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<Guid> GetRequestHeaderFirstParameterAsGuid(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Headers.TryGetValue(parameterName, out var value))
        {
            return Guid.TryParse(value.First(), out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<Guid>($"Unable to parse Request Header Parameter - '{parameterName}' to Guid: '{value}'");
        }

        return Result.Fail<Guid>($"Unable to get Request Header Parameter - '{parameterName}'. Request Header Parameters available are '{httpContext.Request.Headers.Keys.StringJoin(",")}'");
    }
}