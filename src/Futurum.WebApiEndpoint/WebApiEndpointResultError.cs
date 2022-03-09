using System.Net;

using Futurum.Core.Result;

using Microsoft.AspNetCore.WebUtilities;

namespace Futurum.WebApiEndpoint;

public class WebApiEndpointResultError : IResultErrorNonComposite
{
    public WebApiEndpointResultError(HttpStatusCode httpStatusCode, string detailErrorMessage)
    {
        HttpStatusCode = httpStatusCode;
        DetailErrorMessage = detailErrorMessage;
    }

    public HttpStatusCode HttpStatusCode { get; }
    public string DetailErrorMessage { get; }

    public string GetErrorString() =>
        $"{ReasonPhrases.GetReasonPhrase((int)HttpStatusCode)} - {DetailErrorMessage}";

    public ResultErrorStructure GetErrorStructure() => 
        new($"{ReasonPhrases.GetReasonPhrase((int)HttpStatusCode)} - {DetailErrorMessage}", Enumerable.Empty<ResultErrorStructure>());
}