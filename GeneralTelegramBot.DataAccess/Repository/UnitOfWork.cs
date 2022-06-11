using GeneralTelegramBot.DataAccess.Data;
using GeneralTelegramBot.DataAccess.Repository.IRepository;

namespace GeneralTelegramBot.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    public UnitOfWork()
    {
        PhotoRepository = new PhotoRepository(DbContext);
        UserRepository = new UserRepository(DbContext);
        MessageRepository = new MessageRepository(DbContext);

    }

    public IPhotoRepository PhotoRepository { get; }
    public IUserRepository UserRepository { get; }
    public IMessageRepository MessageRepository { get; }

    public void Save()
    {
        DbContext.SaveChanges();
    }

    public GeneralTelegramBotDbContext DbContext { get; } = new GeneralTelegramBotDbContext();

    private bool disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                DbContext.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}