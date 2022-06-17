using GeneralTelegramBot.Commands;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace GeneralTelegramBot;

public class Handlers
{
    private static bool IsMultiplePhotos;

    public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;

        if (message == null)
        {
            return;
        }

        if (IsReplyMessageUpdate(message))
        {
            switch (message.Text!.Split(' ', '@')[0])
            {
                case "/save":
                    if (message.ReplyToMessage!.Photo != null)
                    {
                        await SavePhotoCommand.Execute(botClient, message, message.ReplyToMessage.Photo[^1].FileId);
                    }
                    else if (message.ReplyToMessage.Text != null)
                    {
                        await SaveMessageCommand.Execute(botClient, message, cancellationToken);
                    }
                    break;
                case "/audio":
                    if (message.ReplyToMessage.Text != null)
                    {
                        await AudioCommand.Execute(botClient, message, cancellationToken);
                    }
                    break;
            }
        }
        else if (message.Text != null)
        {
            IsMultiplePhotos = false;
            await HandleTextCommand(botClient, message, cancellationToken);
        }
        else if (IsMessageUpdate(message))
        {
            if (IsMultiplePhotos)
            {
                message.Caption = "/save";
            }

            if (message.Caption!.Contains("/save"))
            {
                IsMultiplePhotos = true;
                await SavePhotoCommand.Execute(botClient, message, message.Photo[^1].FileId);
            }
        }
    }

    private static async Task HandleTextCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        switch (message.Text!.Split(' ', '@')[0])
        {
            case "/healthcheck":
                await HealthCheckCommand.Execute(botClient, message, cancellationToken);
                break;
            case "/help":
                await HelpCommand.Execute(botClient, message, cancellationToken);
                break;
            case "/random":
                await RandomCommand.Execute(botClient, message, cancellationToken);
                break;
            case "/anek":
                await AnecdoteCommand.Execute(botClient, message, cancellationToken);
                break;
            case "/aanek":
                await AudioAnecdoteCommand.Execute(botClient, message, cancellationToken);
                break;
        }
    }

    private static bool IsMessageUpdate(Message message)
    {
        return message.Photo != null &&
               message.Caption != null;
    }

    private static bool IsReplyMessageUpdate(Message message)
    {
        return message.ReplyToMessage != null &&
               message.Text != null;
    }
}