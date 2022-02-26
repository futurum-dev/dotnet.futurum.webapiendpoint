namespace Futurum.WebApiEndpoint.Sample.Features;

public class FeatureMapper : IWebApiEndpointResponseMapper<Feature, FeatureDto>
{
    private readonly IWebApiEndpointResponseDataMapper<Feature, FeatureDto> _dataMapper;

    public FeatureMapper(IWebApiEndpointResponseDataMapper<Feature, FeatureDto> dataMapper)
    {
        _dataMapper = dataMapper;
    }
    
    public FeatureDto Map(HttpContext httpContext, Feature domain) =>
        _dataMapper.Map(domain);
}