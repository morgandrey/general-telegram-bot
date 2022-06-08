using Telegram.Bot;
using Telegram.Bot.Types;

namespace ImgTelegramBot.Commands;

public static class HealthCheckCommand
{
    public static async Task Execute(ITelegramBotClient botClient, Message message)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Alive!");
    }
}