namespace Futurum.WebApiEndpoint;

/// <summary>
/// Request dto for plain json
/// </summary>
public record RequestJsonDto<TRequestDto> : IRequestWrapperDto<TRequestDto>;