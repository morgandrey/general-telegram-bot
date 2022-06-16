using GeneralTelegramBot.DataAccess.Models;

namespace GeneralTelegramBot.DataAccess.Repository.IRepository;

public interface IUserRepository : IRepository<User>
{
    User TryAddUser(User user);
}