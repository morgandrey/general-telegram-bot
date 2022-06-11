using GeneralTelegramBot.DataAccess.Data;

namespace GeneralTelegramBot.DataAccess.Repository.IRepository;

public interface IUnitOfWork
{
    void Save();
    GeneralTelegramBotDbContext DbContext { get; }
    IPhotoRepository PhotoRepository { get; }
    IUserRepository UserRepository { get; }
    IMessageRepository MessageRepository { get; }
}