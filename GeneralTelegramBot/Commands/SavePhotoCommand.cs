using GeneralTelegramBot.Contracts;
using GeneralTelegramBot.DataAccess.Models;
using GeneralTelegramBot.DataAccess.Repository.IRepository;
using Telegram.Bot;
using TelegramMessage = Telegram.Bot.Types.Message;

namespace GeneralTelegramBot.Commands;

public class SavePhotoCommand : TelegramCommand
{
    private readonly IUnitOfWork unitOfWork;

    public override string Name => "/save";

    public SavePhotoCommand(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public override async Task Execute(ITelegramBotClient botClient, TelegramMessage message, CancellationToken cancellationToken)
    {
        var telegramFileId = message.ReplyToMessage == null ? message.Photo[^1].FileId : message.ReplyToMessage.Photo[^1].FileId;
        var chatId = message.Chat.Id;
        try
        {
            await using var outputMemoryStream = new MemoryStream();
            await botClient.GetInfoAndDownloadFileAsync(
                telegramFileId,
                outputMemoryStream,
                cancellationToken: cancellationToken);

            var byteArray = outputMemoryStream.ToArray();
            SavePhotoToDb(message, byteArray);

            await botClient.SendTextMessageAsync(
                chatId,
                "Photos saved successfully!",
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await botClient.SendTextMessageAsync(
                chatId,
                "An error occurred while saving photos!",
                cancellationToken: cancellationToken);
        }
    }

    private void SavePhotoToDb(TelegramMessage message, byte[] photoByteArray)
    {
        var photoAlreadyInDb =
            unitOfWork.PhotoRepository.GetFirstOrDefault(x => x.PhotoSource.SequenceEqual(photoByteArray));

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
            PhotoSource = photoByteArray,
            PhotoCreationDate = DateTime.Now,
            UserId = user.UserId
        };

        unitOfWork.PhotoRepository.Add(photo);
        unitOfWork.Save();
    }

    public override bool Contains(TelegramMessage message)
    {
        return (message.ReplyToMessage != null &&
                message.Text != null &&
                message.ReplyToMessage!.Photo != null &&
                message.Text.Split(' ', '@')[0].Contains(Name)) || 
               (message.Caption != null &&
                message.Photo != null &&
                message.Caption.Split(' ', '@')[0].Contains(Name));

    }
}