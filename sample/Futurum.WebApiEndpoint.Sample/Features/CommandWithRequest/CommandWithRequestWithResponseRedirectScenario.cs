using FluentValidation;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequest;

public static class CommandWithRequestWithResponseRedirectScenario
{
    public record CommandDto(string Id);
    
    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.Request<CommandDto, Command>.ResponseRedirect.Mapper<Mapper>
    {
        public override Task<Result<ResponseRedirect>> ExecuteAsync(Command request, CancellationToken cancellationToken) =>
            new ResponseRedirect("api/1.0/command-with-request-with-response", true).ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
    {
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CommandDto dto, CancellationToken cancellationToken) =>
            new Command(dto.Id).ToResultOkAsync();
    }

    public class Validator : AbstractValidator<CommandDto>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage($"must have a value;");
        }
    }
}