using GeneralTelegramBot.DataAccess.Data;
using GeneralTelegramBot.DataAccess.Models;
using GeneralTelegramBot.DataAccess.Repository.IRepository;

namespace GeneralTelegramBot.DataAccess.Repository;

public class MessageRepository : IMessageRepository
{
    private readonly GeneralTelegramBotDbContext dbContext;

    public MessageRepository(GeneralTelegramBotDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Message GetMessageByUserName(string userName)
    {
        throw new NotImplementedException();
    }

    public void InsertMessage(Message message)
    {
        dbContext.Messages.Add(message);
    }
}