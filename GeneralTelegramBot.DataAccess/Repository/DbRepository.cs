using GeneralTelegramBot.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace GeneralTelegramBot.DataAccess.Repository;

public class DbRepository
{
    private readonly GeneralTelegramBotDbContext dbContext;

    public DbRepository(GeneralTelegramBotDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<User> CreateUser(string firstName, string lastName, string userName)
    {
        var userAlreadyExists = FindUserByUsername(userName).Result;
        if (userAlreadyExists != null)
        {
            return userAlreadyExists;
        }

        var user = new User
        {
            UserName = firstName,
            UserSurname = lastName,
            UserLogin = userName
        };

        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return user;

    }

    public async void SaveTelegramMessage(Message message)
    {
        await dbContext.AddAsync(message);
        await dbContext.SaveChangesAsync();
    }

    public async Task<User> FindUserByUsername(string userName)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.UserLogin == userName);
        return user;
    }
}