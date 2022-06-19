using GeneralTelegramBot.Commands;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace GeneralTelegramBot;

public class TelegramMessageHandler
{
    private readonly ITelegramBotClient botClient;
    private static bool IsMultiPhotoSave;

    public TelegramMessageHandler(ITelegramBotClient botClient)
    {
        this.botClient = botClient;
    }

    public Task HandleErrorAsync(Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;

        if (message == null)
        {
            return;
        }

        try
        {
            if (IsReplyMessageUpdate(message))
            {
                switch (message.Text!.Split(' ', '@')[0])
                {
                    case "/save":
                        if (message.ReplyToMessage!.Photo != null)
                        {
                            await SavePhotoCommand.Execute(botClient, message, message.ReplyToMessage.Photo[^1].FileId,
                                cancellationToken);
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
                IsMultiPhotoSave = false;
                await HandleTextCommand(message, cancellationToken);
            }
            else if (IsMessageUpdate(message))
            {
                if (IsMultiPhotoSave)
                {
                    message.Caption = "/save";
                }

                if (message.Caption != null && message.Caption!.Contains("/save"))
                {
                    IsMultiPhotoSave = true;
                    await SavePhotoCommand.Execute(botClient, message, message.Photo[^1].FileId, cancellationToken);
                }
            }
        }
        catch (Exception exception)
        {
            await HandleErrorAsync(exception, cancellationToken);
        }
    }

    private async Task HandleTextCommand(Message message, CancellationToken cancellationToken)
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

    private bool IsMessageUpdate(Message message)
    {
        return message.Photo != null;
    }

    private bool IsReplyMessageUpdate(Message message)
    {
        return message.ReplyToMessage != null &&
               message.Text != null;
    }
}