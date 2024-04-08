using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace SpaceBattle.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RabbitMqController : ControllerBase
{

    private readonly IBus _bus;
    public RabbitMqController(IBus bus)
    {
        _bus = bus;
    }

    [HttpPost()]
    public IActionResult Post(MyMessage message)
    {
        _bus.Publish(message);
        return Ok();
    }
}
