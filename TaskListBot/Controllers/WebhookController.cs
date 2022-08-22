using Microsoft.AspNetCore.Mvc;
using TaskListBot.Database;
using TaskListBot.Services;
using Telegram.Bot.Types;

namespace TaskListBot.Controllers;

public class WebhookController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromServices] HandleUpdateService handleUpdateService,
        [FromBody] Update update, ApplicationContext db)
    {
        await handleUpdateService.EchoAsync(update, db);
        return Ok();
    }
}