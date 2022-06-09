using GeneralTelegramBot.Models;
using GeneralTelegramBot.Repository;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using YandexDisk.Client.Clients;
using YandexDisk.Client.Http;
using TelegramMessage = Telegram.Bot.Types.Message;
using SystemFile = System.IO.File;
using User = GeneralTelegramBot.Models.User;

namespace GeneralTelegramBot.Commands;

public class SaveImageCommand
{
    private const string YandexDiskUrl = "https://disk.yandex.ru/d/QUVXM648JvB5cQ";

    private static async Task UploadToYandexDisk(string diskPath, string localPath)
    {
        await new DiskHttpApi(Constants.YandexDiskToken).Files.UploadFileAsync(diskPath, false, localPath, CancellationToken.None);
    }

    private static async Task DownloadToYandexDisk(string DiskPath, string LocalPath) =>
        await new DiskHttpApi(Constants.YandexDiskToken).Files.DownloadFileAsync(DiskPath, LocalPath);


    public static async Task Execute(ITelegramBotClient botClient, TelegramMessage message, string image)
    {
        var chatId = message.Chat.Id;
        var tempFilePath = CreateTempFilePath();
        try
        {
            await using var outputFileStream = new FileStream(tempFilePath, FileMode.Create);
            await botClient.GetInfoAndDownloadFileAsync(image, outputFileStream);
            await outputFileStream.DisposeAsync();

            await SaveImageToDb(message, tempFilePath);
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

    private static string CreateTempFilePath()
    {
        var fileName = DateTime.Now.ToString("MM-dd-yyyy;hh-mm-sstt");
        return Path.Combine(Environment.CurrentDirectory, $"{fileName}.jpg");
    }

    private static async Task SaveImageToDb(TelegramMessage message, string pathToFile)
    {
        await using var dbContext = new GeneralTelegramBotDbContext();
        var dbRepository = new DbRepository(dbContext);
        var user = dbRepository.FindUserByUsername(message.From.Username).Result ?? dbRepository.CreateUser(message).Result;
        var byteArray = await SystemFile.ReadAllBytesAsync(pathToFile);
        var photo = new Photo
        {
            PhotoHash = Guid.NewGuid().ToString(),
            PhotoSource = byteArray,
            PhotoCreationDate = DateTime.Now,
            UserId = user.UserId
        };
        await dbContext.AddAsync(photo);
        await dbContext.SaveChangesAsync();
    }
}