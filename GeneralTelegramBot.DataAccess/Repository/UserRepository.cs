using GeneralTelegramBot.DataAccess.Data;
using GeneralTelegramBot.DataAccess.Models;
using GeneralTelegramBot.DataAccess.Repository.IRepository;

namespace GeneralTelegramBot.DataAccess.Repository;

public class UserRepository : IUserRepository
{
    private readonly GeneralTelegramBotDbContext dbContext;

    public UserRepository(GeneralTelegramBotDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public User GetUserByUserName(string userName)
    {
        var user = dbContext.Users.FirstOrDefault(x => x.UserLogin == userName);
        return user;
    }

    public void InsertUser(User user)
    {
        var userAlreadyExists = GetUserByUserName(user.UserName);
        if (userAlreadyExists != null)
        {
            return;
        }
        dbContext.Users.Add(user);
    }
}