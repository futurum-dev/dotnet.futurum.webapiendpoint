using System.Text.Json.Serialization;

using FluentValidation;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Benchmark.WebApiEndpoint;

public static class TestWebApiEndpoint
{
    public record RequestDto(string FirstName, string LastName, int Age, IEnumerable<string> PhoneNumbers)
    {
        [MapFromPath("Id")] public int Id { get; set; }
    }

    public record ResponseDto(int Id, string Name, int Age, string PhoneNumber);

    public record Query(int Id, string FirstName, string LastName, int Age, IEnumerable<string> PhoneNumbers);

    public record Response(int Id, string Name, int Age, string PhoneNumber);

    public class ApiEndpoint : CommandWebApiEndpoint.Request<RequestDto, Query>.Response<ResponseDto, Response>.Mapper<Mapper>
    {
        public override async Task<Result<Response>> ExecuteAsync(Query request, CancellationToken cancellationToken) =>
            new Response(request.Id, request.FirstName + " " + request.LastName, request.Age, request.PhoneNumbers?.FirstOrDefault()).ToResultOk();
    }

    public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Query>, IWebApiEndpointResponseDtoMapper<Response, ResponseDto>
    {
        public Task<Result<Query>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, RequestDto dto, CancellationToken cancellationToken) =>
            new Query(dto.Id, dto.FirstName, dto.LastName, dto.Age, dto.PhoneNumbers).ToResultOkAsync();

        public ResponseDto Map(Response domain) => 
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

[JsonSerializable(typeof(TestWebApiEndpoint.RequestDto))]
[JsonSerializable(typeof(TestWebApiEndpoint.ResponseDto))]
public partial class WebApiEndpointJsonSerializerContext : JsonSerializerContext
{
}