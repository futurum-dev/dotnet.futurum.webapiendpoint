namespace Futurum.WebApiEndpoint;

public class ResponseEmpty
{
    private ResponseEmpty()
    {
    }

    public static readonly ResponseEmpty Default = new();
}

public class ResponseEmptyDto
{
    private ResponseEmptyDto()
    {
    }

    public static readonly ResponseEmptyDto Default = new();
}