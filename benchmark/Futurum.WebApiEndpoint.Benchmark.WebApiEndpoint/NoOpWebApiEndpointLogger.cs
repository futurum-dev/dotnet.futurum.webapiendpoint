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

    public void ErrorUnhandled(Exception exception, string route, string path, string status, int statusCode, string reason)
    {
    }

    public void ErrorWebApiEndpointNotFound(string path, string httpMethod)
    {
    }

    public void EndpointConfiguring(string route, MetadataRouteDefinition metadataRouteDefinition)
    {
    }
}