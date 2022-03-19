using System.Net;

using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public static class WebApiEndpointResultErrorExtensions
{
    public static IResultError ToResultError(this HttpStatusCode httpStatusCode) =>
        new WebApiEndpointResultError(httpStatusCode, ResultErrorEmpty.Value);

    public static IResultError ToResultError(this HttpStatusCode httpStatusCode, string detailErrorMessage) =>
        new WebApiEndpointResultError(httpStatusCode, detailErrorMessage);

    public static IResultError ToResultError(this HttpStatusCode httpStatusCode, IResultError error) =>
        new WebApiEndpointResultError(httpStatusCode, error);
}