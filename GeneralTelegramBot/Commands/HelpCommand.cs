using Telegram.Bot;
using Telegram.Bot.Types;

namespace GeneralTelegramBot.Commands;

public static class HelpCommand
{
    private const string CommandMessage = "Available commands:\n" +
                                          "/healthcheck\n" +
                                          "/anek (Random anecdote)\n" +
                                          "/random (Random cat image)\n" +
                                          "/save (Save photo or message. Usage: save message - reply to a text message, save photo - reply to a photo or in the photo caption)\n" +
                                          "/audio (Converts text message to voice. Usage: Reply to a text message)";

    public static async Task Execute(ITelegramBotClient botClient, Message message)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, CommandMessage);
    }
}