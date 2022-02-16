using FluentValidation;

using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class CommandWithRequestWithResponseScenario
{
    public record CommandDto(string Id);
    
    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponse<FeatureDto, Feature>.WithMapper<Mapper, FeatureMapper>
    {
        protected override Task<Result<Feature>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            new Feature($"Name - {command.Id}").ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
    {
        public Result<Command> Map(HttpContext httpContext, CommandDto dto) =>
            new Command(dto.Id).ToResultOk();
    }

    public class Validator : AbstractValidator<CommandDto>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage($"must have a value;");
        }
    }
}