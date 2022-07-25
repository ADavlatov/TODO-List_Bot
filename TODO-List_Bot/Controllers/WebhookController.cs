using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TODO_List_Bot.Services;

namespace TODO_List_Bot.Controllers;

public class WebhookController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromServices] HandleUpdateService handleUpdateService,
        [FromBody] Update update)
    {
        await handleUpdateService.EchoAsync(update);
        return Ok();
    }
}