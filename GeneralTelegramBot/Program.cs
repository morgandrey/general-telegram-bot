﻿using System.Diagnostics;
using System.Globalization;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using GeneralTelegramBot.Commands;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace GeneralTelegramBot;

public static class Program
{
    private static readonly ITelegramBotClient bot = new TelegramBotClient(Constants.TelegramToken);
    private static bool IsMultiplePhotos;

    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;

        if (message == null)
        {
            return;
        }

        if (IsReplyMessageUpdate(message))
        {
            switch (message.Text)
            {
                case "/save":
                    if (message.ReplyToMessage!.Photo != null)
                    {
                        await SavePhotoCommand.Execute(botClient, message, message.ReplyToMessage.Photo[^1].FileId);
                    }
                    else if (message.ReplyToMessage.Text != null)
                    {
                        await SaveMessageCommand.Execute(botClient, message);
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
            await HandleTextCommand(botClient, message);
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

    private static async Task HandleTextCommand(ITelegramBotClient botClient, Message message)
    {
        switch (message.Text!.ToLower())
        {
            case "/healthcheck":
                await HealthCheckCommand.Execute(botClient, message);
                break;
            case "/help":
                await HelpCommand.Execute(botClient, message);
                break;
            case "/random":
                await RandomCommand.Execute(botClient, message);
                break;
            case "/anek":
                await AnecdoteCommand.Execute(botClient, message);
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

    private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    private static void Main(string[] args)
    {
        Console.WriteLine($"The bot {bot.GetMeAsync().Result.Username} is ready.");

        using var cts = new CancellationTokenSource();
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { },
        };

        bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cts.Token
        );

        Console.ReadLine();
        cts.Cancel();
    }
}