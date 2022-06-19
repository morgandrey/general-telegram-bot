using GeneralTelegramBot.DataAccess.Models;
using GeneralTelegramBot.DataAccess.Repository;
using GeneralTelegramBot.Utils;
using Telegram.Bot;
using TelegramMessage = Telegram.Bot.Types.Message;
using SystemFile = System.IO.File;

namespace GeneralTelegramBot.Commands;

public class SavePhotoCommand
{
    private static readonly UnitOfWork unitOfWork = new UnitOfWork();

    public static async Task Execute(ITelegramBotClient botClient, TelegramMessage message, string image, CancellationToken cancellationToken)
    {
        var chatId = message.Chat.Id;
        var tempFilePath = GeneralUtils.CreateTempFilePath("jpg");
        try
        {
            await using var outputFileStream = new FileStream(tempFilePath, FileMode.Create);
            await botClient.GetInfoAndDownloadFileAsync(image,
                outputFileStream,
                cancellationToken: cancellationToken);
            await outputFileStream.DisposeAsync();
            var byteArray = await SystemFile.ReadAllBytesAsync(tempFilePath, cancellationToken);
            SystemFile.Delete(tempFilePath);
            SavePhotoToDb(message, byteArray);
            await botClient.SendTextMessageAsync(chatId,
                "Photos saved successfully!",
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await botClient.SendTextMessageAsync(chatId,
                "An error occurred while saving photos!",
                cancellationToken: cancellationToken);
        }
    }

    private static void SavePhotoToDb(TelegramMessage message, byte[] byteArray)
    {
        var photoAlreadyInDb =
            unitOfWork.PhotoRepository.GetFirstOrDefault(x => x.PhotoSource.SequenceEqual(byteArray));

        if (photoAlreadyInDb != null)
        {
            return;
        }

        var user = unitOfWork.UserRepository.TryAddUser(new User
        {
            UserName = message.From.FirstName,
            UserSurname = message.From.LastName,
            UserLogin = message.From.Username
        });
        unitOfWork.Save();
        var photo = new Photo
        {
            PhotoHash = Guid.NewGuid().ToString(),
            PhotoSource = byteArray,
            PhotoCreationDate = DateTime.Now,
            UserId = user.UserId
        };

        unitOfWork.PhotoRepository.Add(photo);
        unitOfWork.Save();
    }
}