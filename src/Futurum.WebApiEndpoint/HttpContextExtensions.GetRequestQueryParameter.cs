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
    /// Get <see cref="StringValues"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Query"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<StringValues> GetRequestQueryParameterAsStringValues(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Query.TryGetValue(parameterName, out var value))
        {
            return Result.Ok(value);
        }

        return Result.Fail<StringValues>($"Unable to get Request Query Parameter - '{parameterName}'. Request Query Parameters available are '{httpContext.Request.Query.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="string"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Query"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<string> GetRequestQueryFirstParameterAsString(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Query.TryGetValue(parameterName, out var value))
        {
            var firstValue = value.First();
            return !string.IsNullOrEmpty(firstValue)
                ? Result.Ok(firstValue)
                : Result.Fail<string>($"Unable to parse Request Query Parameter - '{parameterName}' to String: '{value}'");
        }

        return Result.Fail<string>($"Unable to get Request Query Parameter - '{parameterName}'. Request Query Parameters available are '{httpContext.Request.Query.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="int"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Query"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<int> GetRequestQueryFirstParameterAsInt(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Query.TryGetValue(parameterName, out var value))
        {
            return int.TryParse(value.First(), out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<int>($"Unable to parse Request Query Parameter - '{parameterName}' to Int: '{value}'");
        }

        return Result.Fail<int>($"Unable to get Request Query Parameter - '{parameterName}'. Request Query Parameters available are '{httpContext.Request.Query.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="long"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Query"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<long> GetRequestQueryFirstParameterAsLong(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Query.TryGetValue(parameterName, out var value))
        {
            return long.TryParse(value.First(), out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<long>($"Unable to parse Request Query Parameter - '{parameterName}' to Long: '{value}'");
        }

        return Result.Fail<long>($"Unable to get Request Query Parameter - '{parameterName}'. Request Query Parameters available are '{httpContext.Request.Query.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="DateTime"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Query"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<DateTime> GetRequestQueryFirstParameterAsDateTime(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Query.TryGetValue(parameterName, out var value))
        {
            return DateTime.TryParse(value.First(), out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<DateTime>($"Unable to parse Request Query Parameter - '{parameterName}' to DateTime: '{value}'");
        }

        return Result.Fail<DateTime>($"Unable to get Request Query Parameter - '{parameterName}'. Request Query Parameters available are '{httpContext.Request.Query.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="bool"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Query"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<bool> GetRequestQueryFirstParameterAsBool(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Query.TryGetValue(parameterName, out var value))
        {
            return bool.TryParse(value.First(), out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<bool>($"Unable to parse Request Query Parameter - '{parameterName}' to Bool: '{value}'");
        }

        return Result.Fail<bool>($"Unable to get Request Query Parameter - '{parameterName}'. Request Query Parameters available are '{httpContext.Request.Query.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="Guid"/> from <see cref="HttpContext"/> <see cref="HttpRequest.Query"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<Guid> GetRequestQueryFirstParameterAsGuid(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.Query.TryGetValue(parameterName, out var value))
        {
            return Guid.TryParse(value.First(), out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<Guid>($"Unable to parse Request Query Parameter - '{parameterName}' to Guid: '{value}'");
        }

        return Result.Fail<Guid>($"Unable to get Request Query Parameter - '{parameterName}'. Request Query Parameters available are '{httpContext.Request.Query.Keys.StringJoin(",")}'");
    }
}