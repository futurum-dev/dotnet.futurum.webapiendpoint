using Futurum.Core.Option;

namespace Futurum.WebApiEndpoint;

public static class RangeHeaderMapper
{
    public static Option<Range> Map(HttpContext httpContext)
    {
        var typedRequestHeaders = httpContext.Request.GetTypedHeaders();

        var rangeHeaderValue = typedRequestHeaders.Range;

        if (rangeHeaderValue == null || rangeHeaderValue.Ranges.Count == 0)
        {
            return Option<Range>.None;
        }
        
        var firstRange = rangeHeaderValue.Ranges.First();
        
        var from = firstRange.From.ToOption();
        var to = firstRange.To.ToOption();
        
        return new Range(from, to);
    }
}