using System.Diagnostics;
using System.Speech.Synthesis;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GeneralTelegramBot.Commands;

public static class AudioCommand
{
    public static async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var inputWavFilePath = Utils.CreateTempFilePath("wav");
        var outputOggFilePath = Utils.CreateTempFilePath("ogg");
        using var synthesizer = new SpeechSynthesizer();
        synthesizer.SetOutputToDefaultAudioDevice();
        synthesizer.SetOutputToWaveFile(inputWavFilePath);
        synthesizer.Speak(message.ReplyToMessage.Text);
        synthesizer.SetOutputToNull();

        var processStartInfo = new ProcessStartInfo
        {
            CreateNoWindow = false,
            UseShellExecute = false,
            FileName = @"C:\Users\morgu\Desktop\ffmpeg\bin\ffmpeg.exe",
            WindowStyle = ProcessWindowStyle.Hidden,
            Arguments = $"-i {inputWavFilePath} -acodec libvorbis {outputOggFilePath}"
        };
        using var exeProcess = Process.Start(processStartInfo);
        await exeProcess.WaitForExitAsync(cancellationToken);

        await using var stream = System.IO.File.OpenRead(outputOggFilePath);
        await botClient.SendVoiceAsync(
            chatId: message.Chat.Id,
            voice: stream,
            duration: 60,
            cancellationToken: cancellationToken);
    }
}