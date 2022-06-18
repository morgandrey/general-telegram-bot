using System.Diagnostics;
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

    public static async Task ExecuteFFMPEGProcess(string inputWavFilePath, string outputOggFilePath, CancellationToken cancellationToken)
    {
        var processStartInfo = new ProcessStartInfo
        {
            CreateNoWindow = false,
            UseShellExecute = false,
            FileName = Path.Combine(Environment.CurrentDirectory, "FFMPEG/ffmpeg.exe"),
            WindowStyle = ProcessWindowStyle.Hidden,
            Arguments = $"-i {inputWavFilePath} -acodec libopus {outputOggFilePath}"
        };
        using var exeProcess = Process.Start(processStartInfo);
        await exeProcess.WaitForExitAsync(cancellationToken);
    }
}