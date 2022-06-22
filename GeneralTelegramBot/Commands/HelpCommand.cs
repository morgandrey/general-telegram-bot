using GeneralTelegramBot.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GeneralTelegramBot.Commands;

public class HelpCommand : TelegramCommand
{
    private const string CommandMessage = "Available commands:\n" +
                                          "/healthcheck\n" +
                                          "/anek Random anecdote\n" +
                                          "/random Random cat image\n" +
                                          "/save Save photo or message\nSave message reply to a text message, save photo reply to a photo or in the photo caption\n" +
                                          "/audio Converts text message to voice\nReply to a text message";

    public override string Name => "/help";

    public override async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
            CommandMessage,
            ParseMode.MarkdownV2,
            disableNotification: true,
            cancellationToken: cancellationToken);
    }

    public override bool Contains(Message message)
    {
        return message.Text != null &&
               message.Text.Split(' ', '@')[0].Contains(Name);
    }
}