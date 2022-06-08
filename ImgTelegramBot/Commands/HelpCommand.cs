using Telegram.Bot;
using Telegram.Bot.Types;

namespace ImgTelegramBot.Commands;

public static class HelpCommand
{
    public static async Task Execute(ITelegramBotClient botClient, Message message)
    {
        var commandMessage = "Available commands:\n" +
                      "/healthcheck\n" +
                      "/anek\n" +
                      "/random\n";
        await botClient.SendTextMessageAsync(message.Chat.Id, commandMessage);
    }
}