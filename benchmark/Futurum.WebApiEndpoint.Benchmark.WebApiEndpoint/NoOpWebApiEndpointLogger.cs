using Futurum.Core.Result;
using Futurum.WebApiEndpoint;
using Futurum.WebApiEndpoint.Metadata;

public class NoOpWebApiEndpointLogger : IWebApiEndpointLogger
{
    public void ApiEndpointDebugLog(string apiEndpointDebugLog)
    {
    }

    public void RequestReceived<TRequest, TResponse>(TRequest request)
    {
    }

    public void ResponseSent<TRequest, TResponse>(TResponse response)
    {
    }

    public void Error(string path, IResultError error)
    {
    }

    public void Error(Exception exception, IWebApiEndpointLogger.WebApiRouteErrorData errorData)
    {
    }

    public void Error(IWebApiEndpointLogger.WebApiEndpointNotFoundData errorData)
    {
    }

    public void EndpointConfiguring(string route, MetadataRouteDefinition metadataRouteDefinition)
    {
    }
}