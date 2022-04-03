using System.Text.Json.Serialization;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Benchmark.MvcController;

public record RequestDto(string? FirstName, string? LastName, int Age, IEnumerable<string>? PhoneNumbers);

public record ResponseDto(int Id, string? Name, int Age, string? PhoneNumber);

public class TestController : Controller
{
    private readonly IValidator<RequestDto> _validator;

    public TestController(IValidator<RequestDto> validator)
    {
        _validator = validator;
    }

    [AllowAnonymous]
    [HttpPost("api/benchmark/{id}")]
    public async Task<IActionResult> Index([FromRoute] int id, [FromBody] RequestDto requestDto)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(requestDto);

            if (validationResult.IsValid)
            {
                return Ok(new ResponseDto(id, $"{requestDto.FirstName} {requestDto.LastName}", requestDto.Age, requestDto.PhoneNumbers?.FirstOrDefault()));
            }

            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return ValidationProblem(ModelState);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
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

[JsonSerializable(typeof(RequestDto))]
[JsonSerializable(typeof(ResponseDto))]
public partial class WebApiEndpointJsonSerializerContext : JsonSerializerContext
{
}