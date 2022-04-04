using Futurum.Core.Linq;
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
    public static Result<string> GetRequestPathParameterAsString(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.RouteValues.TryGetValue(parameterName, out var value))
        {
            var stringValue = value?.ToString();
            return !string.IsNullOrEmpty(stringValue)
                ? Result.Ok(stringValue)
                : Result.Fail<string>($"Unable to parse Request Path Parameter - '{parameterName}' to String: '{value}'");
        }

        return Result.Fail<string>($"Unable to get Request Path Parameter - '{parameterName}'. Request Path Parameters available are '{httpContext.Request.RouteValues.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="int"/> from <see cref="HttpContext"/> <see cref="HttpRequest.RouteValues"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<int> GetRequestPathParameterAsInt(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.RouteValues.TryGetValue(parameterName, out var value))
        {
            return int.TryParse(value?.ToString(), out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<int>($"Unable to parse Request Path Parameter - '{parameterName}' to Int: '{value}'");
        }

        return Result.Fail<int>($"Unable to get Request Path Parameter - '{parameterName}'. Request Path Parameters available are '{httpContext.Request.RouteValues.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="long"/> from <see cref="HttpContext"/> <see cref="HttpRequest.RouteValues"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<long> GetRequestPathParameterAsLong(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.RouteValues.TryGetValue(parameterName, out var value))
        {
            return long.TryParse(value?.ToString(), out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<long>($"Unable to parse Request Path Parameter - '{parameterName}' to Long: '{value}'");
        }

        return Result.Fail<long>($"Unable to get Request Path Parameter - '{parameterName}'. Request Path Parameters available are '{httpContext.Request.RouteValues.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="DateTime"/> from <see cref="HttpContext"/> <see cref="HttpRequest.RouteValues"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<DateTime> GetRequestPathParameterAsDateTime(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.RouteValues.TryGetValue(parameterName, out var value))
        {
            return DateTime.TryParse(value?.ToString(), out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<DateTime>($"Unable to parse Request Path Parameter - '{parameterName}' to DateTime: '{value}'");
        }

        return Result.Fail<DateTime>($"Unable to get Request Path Parameter - '{parameterName}'. Request Path Parameters available are '{httpContext.Request.RouteValues.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="bool"/> from <see cref="HttpContext"/> <see cref="HttpRequest.RouteValues"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<bool> GetRequestPathParameterAsBool(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.RouteValues.TryGetValue(parameterName, out var value))
        {
            return bool.TryParse(value?.ToString(), out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<bool>($"Unable to parse Request Path Parameter - '{parameterName}' to Bool: '{value}'");
        }

        return Result.Fail<bool>($"Unable to get Request Path Parameter - '{parameterName}'. Request Path Parameters available are '{httpContext.Request.RouteValues.Keys.StringJoin(",")}'");
    }

    /// <summary>
    /// Get <see cref="Guid"/> from <see cref="HttpContext"/> <see cref="HttpRequest.RouteValues"/> for <paramref name="parameterName"/>
    /// </summary>
    public static Result<Guid> GetRequestPathParameterAsGuid(this HttpContext httpContext, string parameterName)
    {
        if (httpContext.Request.RouteValues.TryGetValue(parameterName, out var value))
        {
            return Guid.TryParse(value?.ToString(), out var parsed)
                ? Result.Ok(parsed)
                : Result.Fail<Guid>($"Unable to parse Request Path Parameter - '{parameterName}' to Guid: '{value}'");
        }

        return Result.Fail<Guid>($"Unable to get Request Path Parameter - '{parameterName}'. Request Path Parameters available are '{httpContext.Request.RouteValues.Keys.StringJoin(",")}'");
    }
}