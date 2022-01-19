namespace Futurum.WebApiEndpoint.Sample.Features;

public class FeatureDataMapper : IWebApiEndpointDataMapper<Feature, FeatureDto>
{
    public FeatureDto Map(Feature data) => 
        new(data.Name);
}