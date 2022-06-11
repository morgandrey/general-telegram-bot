using GeneralTelegramBot.DataAccess.Models;
using GeneralTelegramBot.DataAccess.Repository;
using Telegram.Bot;
using TelegramMessage = Telegram.Bot.Types.Message;

namespace GeneralTelegramBot.Commands;

public class SaveMessageCommand
{
    private static readonly UnitOfWork unitOfWork = new UnitOfWork();
    public static async Task Execute(ITelegramBotClient botClient, TelegramMessage telegramMessage)
    {
        await SaveMessageToDb(botClient, telegramMessage);
    }

    private static async Task SaveMessageToDb(ITelegramBotClient botClient, TelegramMessage telegramMessage)
    {
        var chatId = telegramMessage.Chat.Id;

        var saveMessageUser = new User
        {
            UserName = telegramMessage.From.FirstName,
            UserSurname = telegramMessage.From.LastName,
            UserLogin = telegramMessage.From.Username
        };

        unitOfWork.UserRepository.InsertUser(saveMessageUser);
        unitOfWork.Save();

        var messageUser = new User
        {
            UserName = telegramMessage.ReplyToMessage.From.FirstName,
            UserSurname = telegramMessage.ReplyToMessage.From.LastName,
            UserLogin = telegramMessage.ReplyToMessage.From.Username
        };

        unitOfWork.UserRepository.InsertUser(messageUser);
        unitOfWork.Save();

        var message = new Message
        {
            MessageContent = telegramMessage.ReplyToMessage.Text,
            MessageCreationDate = DateTime.Now,
            MessageUserId = messageUser.UserId,
            SaveUserId = saveMessageUser.UserId
        };

        unitOfWork.MessageRepository.InsertMessage(message);
        unitOfWork.Save();
        unitOfWork.Dispose();
        await botClient.SendTextMessageAsync(chatId, "Message saved successfully!");
    }
}