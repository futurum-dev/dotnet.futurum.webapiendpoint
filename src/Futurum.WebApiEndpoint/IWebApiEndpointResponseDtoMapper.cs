namespace Futurum.WebApiEndpoint;

public interface IWebApiEndpointResponseDtoMapper<TResponse, TResponseDto>
{
    /// <summary>
    /// Map from response domain to response dto
    /// </summary>
    TResponseDto Map(TResponse domain);
}