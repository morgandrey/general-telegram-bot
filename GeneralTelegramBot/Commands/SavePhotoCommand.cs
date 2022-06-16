using GeneralTelegramBot.DataAccess.Models;
using GeneralTelegramBot.DataAccess.Repository;
using Telegram.Bot;
using YandexDisk.Client.Clients;
using YandexDisk.Client.Http;
using TelegramMessage = Telegram.Bot.Types.Message;
using SystemFile = System.IO.File;

namespace GeneralTelegramBot.Commands;

public class SavePhotoCommand
{
    private static readonly UnitOfWork unitOfWork = new UnitOfWork();
    private const string YandexDiskUrl = "https://disk.yandex.ru/d/QUVXM648JvB5cQ";

    private static async Task UploadToYandexDisk(string diskPath, string localPath)
    {
        await new DiskHttpApi(Constants.YandexDiskToken).Files.UploadFileAsync(diskPath, false, localPath, CancellationToken.None);
    }

    public static async Task Execute(ITelegramBotClient botClient, TelegramMessage message, string image)
    {
        var chatId = message.Chat.Id;
        var tempFilePath = Utils.CreateTempFilePath("jpg");
        try
        {
            await using var outputFileStream = new FileStream(tempFilePath, FileMode.Create);
            await botClient.GetInfoAndDownloadFileAsync(image, outputFileStream);
            await outputFileStream.DisposeAsync();

            await SavePhotoToDb(message, tempFilePath);
            await UploadToYandexDisk($"disk:/Фотки/{DateTime.Now:MM-dd-yyyy;hh-mm-sstt}", tempFilePath);
            SystemFile.Delete(tempFilePath);
            await botClient.SendTextMessageAsync(chatId, $"Photos saved successfully!\nUrl: {YandexDiskUrl}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await botClient.SendTextMessageAsync(chatId, "An error occurred while saving photos!");
        }
    }

    private static async Task SavePhotoToDb(TelegramMessage message, string pathToFile)
    {
        var user = unitOfWork.UserRepository.TryAddUser(new User
        {
            UserName = message.From.FirstName,
            UserSurname = message.From.LastName,
            UserLogin = message.From.Username
        });
        unitOfWork.Save();
        var byteArray = await SystemFile.ReadAllBytesAsync(pathToFile);
        var photo = new Photo
        {
            PhotoHash = Guid.NewGuid().ToString(),
            PhotoSource = byteArray,
            PhotoCreationDate = DateTime.Now,
            UserId = user.UserId
        };

        unitOfWork.PhotoRepository.Add(photo);
        unitOfWork.Save();
        unitOfWork.Dispose();
    }
}