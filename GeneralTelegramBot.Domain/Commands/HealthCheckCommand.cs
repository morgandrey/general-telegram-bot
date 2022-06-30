using GeneralTelegramBot.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GeneralTelegramBot.Commands;

public class HealthCheckCommand : TelegramCommand
{
    public override string Name => "/healthcheck";

    public override async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
            "Alive!",
            disableNotification: false,
            cancellationToken: cancellationToken);
    }

    public override bool Contains(Message message)
    {
        return message.Text != null &&
               message.Text.Split(' ', '@')[0].Contains(Name);
    }
}