using FluentValidation;

using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Benchmark.WebApiEndpoint;

public static class TestWebApiEndpoint
{
    public record RequestDto(string? FirstName, string? LastName, int Age, IEnumerable<string>? PhoneNumbers)
    {
        [MapFromPath("Id")] public int Id { get; set; }
    }

    public record ResponseDto(int Id, string? Name, int Age, string? PhoneNumber);

    public record Query(int Id, string? FirstName, string? LastName, int Age, IEnumerable<string>? PhoneNumbers);

    public record Response(int Id, string? Name, int Age, string? PhoneNumber);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<RequestDto, Query>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
    {
        protected override Task<Result<Response>> ExecuteAsync(Query query, CancellationToken cancellationToken) =>
            new Response(query.Id, query.FirstName + " " + query.LastName, query.Age, query.PhoneNumbers?.FirstOrDefault()).ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Query>, IWebApiEndpointResponseMapper<Response, ResponseDto>
    {
        public Result<Query> Map(HttpContext httpContext, RequestDto dto) =>
            new Query(dto.Id, dto.FirstName, dto.LastName, dto.Age, dto.PhoneNumbers).ToResultOk();

        public ResponseDto Map(HttpContext httpContext, Response domain) => 
            new(domain.Id, domain.Name, domain.Age, domain.PhoneNumber);
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
}