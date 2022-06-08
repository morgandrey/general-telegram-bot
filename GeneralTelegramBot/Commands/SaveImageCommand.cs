using Telegram.Bot;
using Telegram.Bot.Types;
using YandexDisk.Client.Clients;
using YandexDisk.Client.Http;

namespace GeneralTelegramBot.Commands
{
    public static class SaveImageCommand
    {
        private const string YandexDiskUrl = "https://disk.yandex.ru/d/QUVXM648JvB5cQ";

        private static async void UploadToYandexDisk(string diskPath, string localPath)
        {
            await new DiskHttpApi(Constants.YandexDiskToken).Files.UploadFileAsync(diskPath, false, localPath, CancellationToken.None);
            System.IO.File.Delete(localPath); // todo possible bug
        }

        private static async void DownloadToYandexDisk(string DiskPath, string LocalPath) =>
            await new DiskHttpApi(Constants.YandexDiskToken).Files.DownloadFileAsync(DiskPath, LocalPath);


        public static async Task Execute(ITelegramBotClient botClient, Message message, string image)
        {
            await SaveImagesToYandexDisc(botClient, message.Chat.Id, image);
        }

        private static async Task SaveImagesToYandexDisc(ITelegramBotClient botClient, long chatId, string image)
        {
            try
            {
                var imageNameDateTime = DateTime.Now.ToString("MM-dd-yyyy;hh-mm-sstt");
                var telegramImage = await botClient.GetFileAsync(image);
                var path = Path.Combine(Environment.CurrentDirectory, $"{imageNameDateTime}.jpg");
                await using var outputFileStream = new FileStream(path, FileMode.Create);
                await botClient.DownloadFileAsync(telegramImage.FilePath, outputFileStream);
                UploadToYandexDisk($"disk:/Фотки/{imageNameDateTime}", path);
                await botClient.SendTextMessageAsync(chatId, $"Photos saved successfully!\nUrl: {YandexDiskUrl}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await botClient.SendTextMessageAsync(chatId, "An error occurred while saving photos!");
            }
        }
    }
}