using Futurum.Core.Option;

namespace Futurum.WebApiEndpoint;

public record Range(Option<long> From, Option<long> To);