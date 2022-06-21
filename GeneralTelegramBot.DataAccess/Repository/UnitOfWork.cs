using GeneralTelegramBot.DataAccess.Data;
using GeneralTelegramBot.DataAccess.Repository.IRepository;

namespace GeneralTelegramBot.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly GeneralTelegramBotDbContext dbContext;
    public UnitOfWork(GeneralTelegramBotDbContext dbContext)
    {
        this.dbContext = dbContext;
        PhotoRepository = new PhotoRepository(dbContext);
        UserRepository = new UserRepository(dbContext);
        MessageRepository = new MessageRepository(dbContext);

    }

    public IPhotoRepository PhotoRepository { get; }
    public IUserRepository UserRepository { get; }
    public IMessageRepository MessageRepository { get; }

    public void Save()
    {
        dbContext.SaveChanges();
    }
}