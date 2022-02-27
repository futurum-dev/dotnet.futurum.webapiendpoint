using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Sample;

public static class WebApiEndpointVersions
{
    public static ApiVersion V1_0 = new(1, 0);
    public static ApiVersion V2_0 = new(2, 0);
    public static ApiVersion V3_0 = new(3, 0);
}