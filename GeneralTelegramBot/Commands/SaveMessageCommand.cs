using GeneralTelegramBot.Models;
using GeneralTelegramBot.Repository;
using Telegram.Bot;
using TelegramMessage = Telegram.Bot.Types.Message;

namespace GeneralTelegramBot.Commands;

public class SaveMessageCommand
{
    public static async Task Execute(ITelegramBotClient botClient, TelegramMessage telegramMessage)
    {
        await SaveMessageToDb(telegramMessage);
    }

    private static async Task SaveMessageToDb(TelegramMessage telegramMessage)
    {
        await using var dbContext = new GeneralTelegramBotDbContext();
        var dbRepository = new DbRepository(dbContext);
        var user = dbRepository.FindUserByUsername(telegramMessage.From.Username).Result ??
                   dbRepository.CreateUser(telegramMessage).Result;
        var message = new Message
        {
            MessageContent = telegramMessage.ReplyToMessage.Text,
            MessageCreationDate = DateTime.Now,
            UserId = user.UserId
        };
        await dbContext.AddAsync(message);
        await dbContext.SaveChangesAsync();
    }
}