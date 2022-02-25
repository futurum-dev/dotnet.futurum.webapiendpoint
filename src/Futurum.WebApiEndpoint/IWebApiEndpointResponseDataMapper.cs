namespace Futurum.WebApiEndpoint;

/// <summary>
/// <see cref="WebApiEndpoint"/> response data mapper
/// </summary>
public interface IWebApiEndpointResponseDataMapper<TData, TDataDto>
{
    /// <summary>
    /// Map from data domain to data dto
    /// </summary>
    TDataDto Map(TData data);
}