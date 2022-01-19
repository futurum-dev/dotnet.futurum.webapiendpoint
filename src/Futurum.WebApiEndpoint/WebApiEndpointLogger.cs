using Futurum.ApiEndpoint;
using Futurum.Core.Result;

using ILogger = Serilog.ILogger;

namespace Futurum.WebApiEndpoint;

public interface IWebApiEndpointLogger : IApiEndpointLogger
{
    void RequestReceived<TRequest, TResponse>(TRequest request);

    void ResponseSent<TRequest, TResponse>(TResponse response);

    void Error(string path, IResultError error);
}

internal class WebApiEndpointLogger : IWebApiEndpointLogger
{
    private readonly ILogger _logger;

    public WebApiEndpointLogger(ILogger logger)
    {
        _logger = logger;
    }

    public void RequestReceived<TRequest, TResponse>(TRequest request)
    {
        var eventData = new RequestReceivedData<TRequest>(typeof(TRequest), typeof(TResponse), request);

        _logger.Debug("WebApiEndpoint request received {@eventData}", eventData);
    }

    public void ResponseSent<TRequest, TResponse>(TResponse response)
    {
        var eventData = new ResponseSentData<TResponse>(typeof(TRequest), typeof(TResponse), response);

        _logger.Debug("WebApiEndpoint response sent {@eventData}", eventData);
    }

    public void Error(string path, IResultError error)
    {
        var eventData = new ErrorData(path, error.ToErrorString());

        _logger.Error("WebApiEndpoint error {@eventData}", eventData);
    }

    public void ApiEndpointDebugLog(string apiEndpointDebugLog)
    {
        var eventData = new ApiEndpoints(apiEndpointDebugLog);

        _logger.Debug("WebApiEndpoint endpoints {@eventData}", eventData);
    }

    private record struct RequestReceivedData<TRequest>(Type RequestType, Type ResponseType, TRequest Request);

    private record struct ResponseSentData<TResponse>(Type RequestType, Type ResponseType, TResponse Response);

    private record struct ErrorData(string Path, string Error);
    
    private record struct ApiEndpoints(string Log);
}