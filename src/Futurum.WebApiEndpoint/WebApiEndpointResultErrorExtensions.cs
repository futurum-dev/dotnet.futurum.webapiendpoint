using System.Net;

using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public static class WebApiEndpointResultErrorExtensions
{
    public static IResultError ToResultError(this HttpStatusCode httpStatusCode) =>
        new WebApiEndpointResultError(httpStatusCode, string.Empty);

    public static IResultError ToResultError(this HttpStatusCode httpStatusCode, string detailErrorMessage) =>
        new WebApiEndpointResultError(httpStatusCode, detailErrorMessage);
}