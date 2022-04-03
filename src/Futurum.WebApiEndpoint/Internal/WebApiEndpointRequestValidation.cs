using FluentValidation;

using Futurum.Core.Result;
using Futurum.FluentValidation;

namespace Futurum.WebApiEndpoint.Internal;

public interface IWebApiEndpointRequestValidation<TRequest>
{
    Task<Result> ExecuteAsync(TRequest request);
}

internal class WebApiEndpointRequestValidation<TRequest> : IWebApiEndpointRequestValidation<TRequest>
{
    private readonly IValidator<TRequest>[] _validator;

    public WebApiEndpointRequestValidation(IEnumerable<IValidator<TRequest>> validator)
    {
        _validator = validator.ToArray();
    }

    public Task<Result> ExecuteAsync(TRequest request) =>
        _validator.Length switch
        {
            0 => Result.OkAsync(),
            1 => ValidateAsync(_validator[0], request),
            _ => _validator.FlatMapAsync(validator => ValidateAsync(validator, request))
        };

    private static async Task<Result> ValidateAsync(IValidator<TRequest> validator, TRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);

        return validationResult.IsValid
            ? Result.Ok()
            : Result.Fail(validationResult.ToResultError());
    }
}