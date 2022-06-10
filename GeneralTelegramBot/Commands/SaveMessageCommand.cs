using GeneralTelegramBot.DataAccess.Models;
using GeneralTelegramBot.DataAccess.Repository;
using Telegram.Bot;
using TelegramMessage = Telegram.Bot.Types.Message;

namespace GeneralTelegramBot.Commands;

public class SaveMessageCommand
{
    public static async Task Execute(ITelegramBotClient botClient, TelegramMessage telegramMessage)
    {
        await SaveMessageToDb(botClient, telegramMessage);
    }

    private static async Task SaveMessageToDb(ITelegramBotClient botClient, TelegramMessage telegramMessage)
    {
        var chatId = telegramMessage.Chat.Id;
        await using var dbContext = new GeneralTelegramBotDbContext();
        var dbRepository = new DbRepository(dbContext);

        var saveUserFirstName = telegramMessage.From.FirstName;
        var saveUserLastName = telegramMessage.From.LastName;
        var saveUserUserName = telegramMessage.From.Username;

        var saveUserId = dbRepository.CreateUser(saveUserFirstName, saveUserLastName, saveUserUserName);

        var messageUserFirstName = telegramMessage.ReplyToMessage.From.FirstName;
        var messageUserLastName = telegramMessage.ReplyToMessage.From.LastName;
        var messageUserUserName = telegramMessage.ReplyToMessage.From.Username;

        var messageUserId = dbRepository.CreateUser(messageUserFirstName, messageUserLastName, messageUserUserName).Result;
        var message = new Message
        {
            MessageContent = telegramMessage.ReplyToMessage.Text,
            MessageCreationDate = DateTime.Now,
            MessageUserId = messageUserId.UserId,
            SaveUserId = saveUserId.Result.UserId
        };
        dbRepository.SaveTelegramMessage(message);
        await botClient.SendTextMessageAsync(chatId, "Message saved successfully!");
    }
}