using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features;

public class PayloadMapper : IWebApiEndpointRequestPayloadMapper<PayloadDto, Payload>
{
    public Result<Payload> Map(PayloadDto dto) =>
        new Payload(dto.Id).ToResultOk();
}