using Telegram.Bot;
using Telegram.Bot.Types;

namespace GeneralTelegramBot.Commands;

public static class HealthCheckCommand
{
    public static async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
            "Alive!",
            disableNotification: false,
            cancellationToken: cancellationToken);
    }
}