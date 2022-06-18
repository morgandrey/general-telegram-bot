using System.Speech.Synthesis;
using GeneralTelegramBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace GeneralTelegramBot.Commands;

public static class AudioCommand
{
    public static async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var inputWavFilePath = GeneralUtils.CreateTempFilePath("wav");
        var outputOggFilePath = GeneralUtils.CreateTempFilePath("ogg");
        CreateAudioWavFile(inputWavFilePath, message.ReplyToMessage.Text);
        await GeneralUtils.ExecuteFFMPEGProcess(inputWavFilePath, outputOggFilePath, cancellationToken);
        var audioDuration = GeneralUtils.GetWavFileDuration(inputWavFilePath).Seconds;
        await using var stream = File.OpenRead(outputOggFilePath);
        await botClient.SendVoiceAsync(
            chatId: message.Chat.Id,
            voice: stream,
            duration: audioDuration,
            disableNotification: true,
            cancellationToken: cancellationToken);
    }

    public static void CreateAudioWavFile(string wavFilePath, string message)
    {
        using var synthesizer = new SpeechSynthesizer();
        synthesizer.SetOutputToDefaultAudioDevice();
        synthesizer.SetOutputToWaveFile(wavFilePath);
        synthesizer.Speak(message);
        synthesizer.SetOutputToNull();
    }
}