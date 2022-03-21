using System.Net;

using Futurum.Core.Result;
using Futurum.FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Futurum.WebApiEndpoint;

public static class ResultErrorProblemDetailsExtensions
{
    public static ProblemDetails ToProblemDetails(this IResultError resultError, int failedStatusCode, string requestPath) =>
        resultError switch
        {
            FluentValidationResultError fluentValidationResultError => FluentValidationResultError(fluentValidationResultError, failedStatusCode, requestPath),
            WebApiEndpointResultError webApiEndpointResultError     => WebApiEndpointResultError(webApiEndpointResultError, requestPath),
            ResultErrorKeyNotFound notFoundResultError              => NotFoundResultError(notFoundResultError, requestPath),
            ResultErrorComposite resultErrorComposite               => ToProblemDetails(resultErrorComposite.Flatten().First(), failedStatusCode, requestPath),
            _                                                       => GeneralError(resultError, failedStatusCode, requestPath)
        };

    private static ProblemDetails FluentValidationResultError(FluentValidationResultError fluentValidationResultError, int failedStatusCode, string requestPath)
    {
        var errors = fluentValidationResultError.ValidationResult.Errors
                                                .GroupBy(x => x.PropertyName)
                                                .ToDictionary(x => x.Key,
                                                              x => x.Select(validationFailure => validationFailure.ErrorMessage).ToArray());

        return new ValidationProblemDetails(errors)
        {
            Detail = fluentValidationResultError.ToErrorString(),
            Instance = requestPath,
            Status = failedStatusCode,
        };
    }

    private static ProblemDetails WebApiEndpointResultError(WebApiEndpointResultError webApiEndpointResultError, string requestPath) =>
        new()
        {
            Detail = webApiEndpointResultError.GetChildrenErrorString(";"),
            Instance = requestPath,
            Status = (int)webApiEndpointResultError.HttpStatusCode,
            Title = webApiEndpointResultError.Parent.Switch(parent => parent.ToErrorString(), () => "Unknown error")
        };

    private static ProblemDetails NotFoundResultError(ResultErrorKeyNotFound resultErrorKeyNotFound, string requestPath) =>
        new()
        {
            Detail = resultErrorKeyNotFound.GetErrorString(),
            Instance = requestPath,
            Status = (int)HttpStatusCode.NotFound,
            Title = ReasonPhrases.GetReasonPhrase((int)HttpStatusCode.NotFound),
        };

    private static ProblemDetails GeneralError(IResultError resultError, int failedStatusCode, string requestPath) =>
        new()
        {
            Detail = resultError.ToErrorString(),
            Instance = requestPath,
            Status = failedStatusCode,
            Title = ReasonPhrases.GetReasonPhrase(failedStatusCode),
        };
}