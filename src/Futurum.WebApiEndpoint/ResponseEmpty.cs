using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Response domain for empty
/// </summary>
public class ResponseEmpty
{
    private ResponseEmpty()
    {
    }

    public static readonly ResponseEmpty Default = new();
    
    public static readonly Task<Result<ResponseEmpty>> DefaultResultAsync = Result.OkAsync(Default);
}

/// <summary>
/// Response dto for empty
/// </summary>
public record ResponseEmptyDto;