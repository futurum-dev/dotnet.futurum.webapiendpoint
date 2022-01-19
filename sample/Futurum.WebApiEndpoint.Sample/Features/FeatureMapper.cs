using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features;

public class FeatureMapper : IWebApiEndpointResponseMapper<Feature, FeatureDto>
{
    private readonly IWebApiEndpointDataMapper<Feature, FeatureDto> _dataMapper;

    public FeatureMapper(IWebApiEndpointDataMapper<Feature, FeatureDto> dataMapper)
    {
        _dataMapper = dataMapper;
    }
    
    public Result<FeatureDto> Map(Feature domain) =>
        _dataMapper.Map(domain).ToResultOk();
}