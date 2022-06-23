using GeneralTelegramBot.Contracts;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace GeneralTelegramBot.Web.Controllers;

public class WebhookController : ControllerBase
{

    private readonly ITelegramBotClient telegramBotClient;
    private readonly ICommandService commandService;

    public WebhookController(ICommandService commandService, ITelegramBotClient telegramBotClient)
    {
        this.commandService = commandService;
        this.telegramBotClient = telegramBotClient;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
    {
        if (update?.Message == null)
        {
            return Ok();
        }

        var message = update.Message;

        foreach (var command in commandService.Get())
        {
            try
            {
                if (!command.Contains(message))
                {
                    continue;
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }

            await command.Execute(telegramBotClient, message, cancellationToken);
            break;
        }

        return Ok();
    }

    public Task HandleErrorAsync(Exception exception)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}
