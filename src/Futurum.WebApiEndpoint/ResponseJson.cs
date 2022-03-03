namespace Futurum.WebApiEndpoint;

/// <summary>
/// Response dto for json
/// </summary>
internal record ResponseJsonDto<TResponseDto> : IResponseWrapperDto<TResponseDto>;