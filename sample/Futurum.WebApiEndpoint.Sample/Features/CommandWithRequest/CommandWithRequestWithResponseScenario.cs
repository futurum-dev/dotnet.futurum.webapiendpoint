using FluentValidation;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequest;

public static class CommandWithRequestWithResponseScenario
{
    public record CommandDto(string Id);
    
    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.Request<CommandDto, Command>.Response<FeatureDto, Feature>.Mapper<Mapper, FeatureMapper>
    {
        public override Task<Result<Feature>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            new Feature($"Name - {command.Id}").ToResultOkAsync();
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