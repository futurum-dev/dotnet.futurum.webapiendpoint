using System.Text.Json.Serialization;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Benchmark.MinimalApi;

public static class TestEndpoint
{
    public static async Task<IResult> Execute([FromRoute] int id, [FromBody] RequestDto requestDto, [FromServices] IValidator<RequestDto> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(requestDto);

            if (validationResult.IsValid)
            {
                return Results.Ok(new ResponseDto(id, requestDto.FirstName + " " + requestDto.LastName, requestDto.Age, requestDto.PhoneNumbers?.FirstOrDefault()));
            }

            var errors = validationResult.Errors
                                         .GroupBy(validationFailure => validationFailure.PropertyName)
                                         .ToDictionary(x => x.Key,
                                                       x => x.Select(validationFailure => validationFailure.ErrorMessage).ToArray());

            return Results.ValidationProblem(errors);
        }
        catch (Exception exception)
        {
            return Results.BadRequest(exception.Message);
        }
    }

    public class Validator : AbstractValidator<RequestDto>
    {
        public Validator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name needed");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Nast needed");
            RuleFor(x => x.Age).GreaterThan(21).WithMessage("You must be at least 18 years old");
            RuleFor(x => x.PhoneNumbers).NotEmpty().WithMessage("Phone Number needed");
        }
    }

    public record RequestDto(string? FirstName, string? LastName, int Age, IEnumerable<string>? PhoneNumbers);

    public record ResponseDto(int Id, string? Name, int Age, string? PhoneNumber);
}

[JsonSerializable(typeof(TestEndpoint.RequestDto))]
[JsonSerializable(typeof(TestEndpoint.ResponseDto))]
public partial class WebApiEndpointJsonSerializerContext : JsonSerializerContext
{
}