using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// <see cref="WebApiEndpoint"/> request payload mapper
/// </summary>
public interface IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
{
    /// <summary>
    /// Map from payload dto to payload domain
    /// </summary>
    Result<TPayload> Map(TPayloadDto dto);
}