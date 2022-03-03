namespace Futurum.WebApiEndpoint;

/// <summary>
/// Request domain for empty
/// </summary>
public class RequestEmpty
{
    private RequestEmpty()
    {
    }
    
    public static readonly RequestEmpty Default = new();
}

/// <summary>
/// Request dto for empty
/// </summary>
public record RequestEmptyDto;