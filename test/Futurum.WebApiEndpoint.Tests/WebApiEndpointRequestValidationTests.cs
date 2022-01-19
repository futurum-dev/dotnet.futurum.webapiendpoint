using FluentValidation;

using Futurum.Core.Result;
using Futurum.Test.Result;
using Futurum.WebApiEndpoint.Internal;

using Xunit;
using Xunit.Abstractions;

namespace Futurum.WebApiEndpoint.Tests;

public class WebApiEndpointRequestValidationTests
{
    private readonly ITestOutputHelper _output;

    public WebApiEndpointRequestValidationTests(ITestOutputHelper output)
    {
        _output = output;
    }

    public class NoValidation
    {
        private record Request;

        [Fact]
        public async Task success()
        {
            var result = await TestRunner();

            result.ShouldBeSuccess();
        }

        private static Task<Result> TestRunner()
        {
            var webApiEndpointRequestValidation = new WebApiEndpointRequestValidation<Request>(Array.Empty<IValidator<Request>>());

            return webApiEndpointRequestValidation.ExecuteAsync(new Request());
        }
    }

    public class OneValidation
    {
        private const string PropertyName = "Name";
        private const string ErrorMessage = "ErrorMessage";

        private record Request(string Name);

        [Fact]
        public async Task when_no_validation_errors_then_success()
        {
            var result = await TestRunner(new ValidatorSuccess());

            result.ShouldBeSuccess();
        }

        [Fact]
        public async Task when_validation_errors_then_failure()
        {
            var result = await TestRunner(new ValidatorFailure());

            result.ShouldBeFailureWithError($"Validation failure for '{PropertyName}' with error : '{ErrorMessage}'");
        }

        private static Task<Result> TestRunner(IValidator<Request> validator)
        {
            var webApiEndpointRequestValidation = new WebApiEndpointRequestValidation<Request>(new[] { validator });

            return webApiEndpointRequestValidation.ExecuteAsync(new Request(Guid.NewGuid().ToString()));
        }

        private class ValidatorSuccess : AbstractValidator<Request>
        {
        }

        private class ValidatorFailure : AbstractValidator<Request>
        {
            public ValidatorFailure()
            {
                RuleFor(x => x.Name).Empty().WithMessage(ErrorMessage);
            }
        }
    }

    public class MoreValidation
    {
        private const string PropertyName = "Name";
        private const string ErrorMessage = "ErrorMessage";

        private record Request(string Name);

        [Fact]
        public async Task when_no_validation_errors_then_success()
        {
            var result = await TestRunner(new ValidatorSuccess());

            result.ShouldBeSuccess();
        }

        [Fact]
        public async Task when_validation_errors_then_failure()
        {
            var result = await TestRunner(new ValidatorFailure());

            var errorMessage = $"Validation failure for '{PropertyName}' with error : '{ErrorMessage}'";
            result.ShouldBeFailureWithError($"{errorMessage};{errorMessage}");
        }

        private static Task<Result> TestRunner(IValidator<Request> validator)
        {
            var webApiEndpointRequestValidation = new WebApiEndpointRequestValidation<Request>(new[] { validator, validator });

            return webApiEndpointRequestValidation.ExecuteAsync(new Request(Guid.NewGuid().ToString()));
        }

        private class ValidatorSuccess : AbstractValidator<Request>
        {
        }

        private class ValidatorFailure : AbstractValidator<Request>
        {
            public ValidatorFailure()
            {
                RuleFor(x => x.Name).Empty().WithMessage(ErrorMessage);
            }
        }
    }
}