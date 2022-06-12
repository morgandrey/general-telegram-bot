using GeneralTelegramBot.DataAccess.Data;
using GeneralTelegramBot.DataAccess.Models;
using GeneralTelegramBot.DataAccess.Repository.IRepository;

namespace GeneralTelegramBot.DataAccess.Repository;

public class MessageRepository : Repository<Message>, IMessageRepository
{
    private readonly GeneralTelegramBotDbContext dbContext;

    public MessageRepository(GeneralTelegramBotDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public Message GetMessageByUserName(string userName)
    {
        throw new NotImplementedException();
    }
}