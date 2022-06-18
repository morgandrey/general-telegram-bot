using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace GeneralTelegramBot.Web.Controllers;

public class WebhookController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromServices] TelegramMessageHandler telegramMessageHandler,
                                          [FromBody] Update update)
    {
        await telegramMessageHandler.HandleUpdateAsync(update, CancellationToken.None);
        return Ok();
    }
}
