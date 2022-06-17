using System.Diagnostics;
using System.Speech.Synthesis;
using HtmlAgilityPack;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace GeneralTelegramBot.Commands;

public static class AudioAnecdoteCommand 
{
    public static async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var inputWavFilePath = Utils.CreateTempFilePath("wav");
        var outputOggFilePath = Utils.CreateTempFilePath("ogg");
        var randomAnecdote = AnecdoteCommand.GetRandomAnecdote();
        AudioCommand.CreateAudioWavFile(inputWavFilePath, randomAnecdote);
        await Utils.ExecuteFFMPEGProcess(inputWavFilePath, outputOggFilePath, cancellationToken);
        var audioDuration = Utils.GetWavFileDuration(inputWavFilePath).Seconds;
        await using var stream = File.OpenRead(outputOggFilePath);
        await botClient.SendVoiceAsync(
            chatId: message.Chat.Id,
            voice: stream,
            duration: audioDuration,
            cancellationToken: cancellationToken);
    }
}