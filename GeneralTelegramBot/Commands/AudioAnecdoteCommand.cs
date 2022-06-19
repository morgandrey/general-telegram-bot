using GeneralTelegramBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace GeneralTelegramBot.Commands;

public static class AudioAnecdoteCommand 
{
    public static async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var inputWavFilePath = GeneralUtils.CreateTempFilePath("wav");
        var outputOggFilePath = GeneralUtils.CreateTempFilePath("ogg");
        var randomAnecdote = AnecdoteCommand.GetRandomAnecdote();
        GeneralUtils.CreateAudioWavFile(inputWavFilePath, randomAnecdote);
        await GeneralUtils.StartFFMPEGProcess(inputWavFilePath, outputOggFilePath, cancellationToken);
        var audioDuration = GeneralUtils.GetWavFileDuration(inputWavFilePath).Seconds;
        await using var stream = File.OpenRead(outputOggFilePath);
        await botClient.SendVoiceAsync(
            chatId: message.Chat.Id,
            voice: stream,
            duration: audioDuration,
            disableNotification: true,
            cancellationToken: cancellationToken);
        await stream.DisposeAsync();
        // File.Delete(inputWavFilePath);
        File.Delete(outputOggFilePath);
    }
}