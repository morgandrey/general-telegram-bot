namespace GeneralTelegramBot;

public static class Utils
{
    public static string CreateTempFilePath(string format)
    {
        var fileName = DateTime.Now.ToString("MM-dd-yyyy;hh-mm-sstt");
        return Path.Combine(Environment.CurrentDirectory, $"{fileName}.{format}");
    }
}