namespace Futurum.WebApiEndpoint.Sample.Features;

public class FeatureDataMapper : IWebApiEndpointResponseDataMapper<Feature, FeatureDto>
{
    public FeatureDto Map(Feature data) => 
        new(data.Name);
}