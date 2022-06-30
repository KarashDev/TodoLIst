using Microsoft.AspNetCore.Mvc;
using TodoLIst.RabbitMQ;

[Route("api/[controller]")]
[ApiController]
public class RabbitMqController : ControllerBase
{
    private readonly IRabbitMqService mqService;

    public RabbitMqController(IRabbitMqService mqService)
    {
        this.mqService = mqService;
    }

    [Route("[action]/{message}")]
    [HttpGet]
    public IActionResult SendMessage(string message)
    {
        mqService.SendMessage(message);

        return Ok("Сообщение отправлено");
    }
}
