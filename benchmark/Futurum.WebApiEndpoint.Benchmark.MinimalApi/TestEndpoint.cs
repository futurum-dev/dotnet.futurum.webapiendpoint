using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Benchmark.MinimalApi;

public static class TestEndpoint
{
    public static IResult Execute([FromRoute] int id, [FromBody] RequestDto requestDto, [FromServices] IValidator<RequestDto> validator)
    {
        validator.Validate(requestDto);

        return Results.Ok(new ResponseDto(id, requestDto.FirstName + " " + requestDto.LastName, requestDto.Age, requestDto.PhoneNumbers?.FirstOrDefault()));
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