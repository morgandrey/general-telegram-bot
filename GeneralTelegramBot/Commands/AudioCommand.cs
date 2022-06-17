using System.Speech.Synthesis;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace GeneralTelegramBot.Commands;

public static class AudioCommand
{
    public static async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var inputWavFilePath = Utils.CreateTempFilePath("wav");
        var outputOggFilePath = Utils.CreateTempFilePath("ogg");
        CreateAudioWavFile(inputWavFilePath, message.ReplyToMessage.Text);
        await Utils.ExecuteFFMPEGProcess(inputWavFilePath, outputOggFilePath, cancellationToken);
        var audioDuration = Utils.GetWavFileDuration(inputWavFilePath).Seconds;
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