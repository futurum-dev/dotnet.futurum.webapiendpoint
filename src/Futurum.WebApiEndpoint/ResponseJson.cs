namespace Futurum.WebApiEndpoint;

/// <summary>
/// Response dto for json
/// </summary>
public record ResponseJsonDto<TResponseDto> : IResponseWrapperDto<TResponseDto>;