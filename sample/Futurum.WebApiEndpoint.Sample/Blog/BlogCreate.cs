using FluentValidation;

using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Blog;

public static class BlogCreate
{
    public record CommandDto(string Url);
    
    public record Command(Blog Blog);

    public class ApiEndpoint : CommandWebApiEndpoint.Request<CommandDto, Command>.Response<BlogDto, Blog>.Mapper<Mapper, BlogMapper>
    {
        private readonly IBlogStorageBroker _storageBroker;

        public ApiEndpoint(IBlogStorageBroker storageBroker)
        {
            _storageBroker = storageBroker;
        }

        public override Task<Result<Blog>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            _storageBroker.AddAsync(command.Blog);
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
    {
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CommandDto dto, CancellationToken cancellationToken) =>
            new Command(new Blog(Option<Id>.None, dto.Url)).ToResultOkAsync();
    }

    public class Validator : AbstractValidator<CommandDto>
    {
        public Validator()
        {
            RuleFor(x => x.Url).NotEmpty().WithMessage($"must have a value;");
        }
    }
}