using GeneralTelegramBot.DataAccess.Models;

namespace GeneralTelegramBot.DataAccess.Repository.IRepository;

public interface IUserRepository
{
    User GetUserByUserName(string userName);
    void InsertUser(User user);
}