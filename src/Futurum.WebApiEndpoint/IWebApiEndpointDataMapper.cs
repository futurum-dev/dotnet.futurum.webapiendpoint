namespace Futurum.WebApiEndpoint;

/// <summary>
/// <see cref="WebApiEndpoint"/> data mapper
/// </summary>
public interface IWebApiEndpointDataMapper<TData, TDataDto>
{
    /// <summary>
    /// Map from data domain to data dto
    /// </summary>
    TDataDto Map(TData data);
}