namespace Futurum.WebApiEndpoint;

public class RequestEmpty
{
    private RequestEmpty()
    {
    }
    
    public static readonly RequestEmpty Default = new();
}

public class RequestEmptyDto
{
    private RequestEmptyDto()
    {
    }
    
    public static readonly RequestEmptyDto Default = new();
}