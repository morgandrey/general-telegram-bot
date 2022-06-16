using GeneralTelegramBot.DataAccess.Data;
using GeneralTelegramBot.DataAccess.Models;
using GeneralTelegramBot.DataAccess.Repository.IRepository;

namespace GeneralTelegramBot.DataAccess.Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly GeneralTelegramBotDbContext dbContext;

    public UserRepository(GeneralTelegramBotDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public User TryAddUser(User user)
    {
        var userExistInDb = GetFirstOrDefault(x => x.UserName == user.UserName);
        if (userExistInDb != null)
        {
            return userExistInDb;
        }
        Add(user);
        return user;
    }
}