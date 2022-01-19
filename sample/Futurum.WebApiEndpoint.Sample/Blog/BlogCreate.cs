using FluentValidation;

using Futurum.Core.Option;
using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Blog;

public static class BlogCreate
{
    public record CommandDto(string Url);
    
    public record Command(Blog Blog);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponse<BlogDto, Blog>
    {
        private readonly IBlogStorageBroker _storageBroker;

        public ApiEndpoint(IBlogStorageBroker storageBroker)
        {
            _storageBroker = storageBroker;
        }

        protected override Task<Result<Blog>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            _storageBroker.AddAsync(command.Blog);
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
    {
        public Result<Command> Map(HttpContext httpContext, CommandDto dto) =>
            new Command(new Blog(Option<Id>.None, dto.Url)).ToResultOk();
    }

    public class Validator : AbstractValidator<CommandDto>
    {
        public Validator()
        {
            RuleFor(x => x.Url).NotEmpty().WithMessage($"must have a value;");
        }
    }
}