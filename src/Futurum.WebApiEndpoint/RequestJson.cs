namespace Futurum.WebApiEndpoint;

/// <summary>
/// Request dto for plain json
/// </summary>
internal record RequestJsonDto<TRequestDto> : IRequestWrapperDto<TRequestDto>;