using System.Speech.Synthesis;
using CliWrap;
using NAudio.Wave;

namespace GeneralTelegramBot.Utils;

public static class GeneralUtils
{
    public static string CreateTempFilePath(string format)
    {
        var fileName = DateTime.Now.ToString("MM-dd-yyyy;hh-mm-sstt");
        return Path.Combine(Environment.CurrentDirectory, $"{fileName}.{format}");
    }

    public static TimeSpan GetWavFileDuration(string fileName)
    {
        var waveFileReader = new WaveFileReader(fileName);
        return waveFileReader.TotalTime;
    }

    public static void CreateAudioWavFile(string wavFilePath, string message)
    {
        using var synthesizer = new SpeechSynthesizer();
        synthesizer.SetOutputToDefaultAudioDevice();
        synthesizer.SetOutputToWaveFile(wavFilePath);
        synthesizer.Speak(message);
        synthesizer.SetOutputToNull();
    }

    public static CommandTask<CommandResult> StartFFMPEGProcess(string inputWavFilePath, string outputOggFilePath, CancellationToken cancellationToken)
    {
        var solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory())!.FullName;
        var ffmpegProcess = Cli.Wrap(Path.Combine(solutionDirectory, "GeneralTelegramBot/FFMPEG/ffmpeg.exe"))
            .WithArguments(args => args
                .Add("-i")
                .Add($"{inputWavFilePath}")
                .Add("-acodec")
                .Add("libopus")
                .Add($"{outputOggFilePath}"))
            .WithValidation(CommandResultValidation.None)
            .ExecuteAsync(cancellationToken);
        return ffmpegProcess;
    }
}