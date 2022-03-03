using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public class ResponseEmpty
{
    private ResponseEmpty()
    {
    }

    public static readonly ResponseEmpty Default = new();
    
    public static readonly Task<Result<ResponseEmpty>> DefaultResultAsync = Result.OkAsync(Default);
}

public class ResponseEmptyDto
{
    private ResponseEmptyDto()
    {
    }

    public static readonly ResponseEmptyDto Default = new();
}