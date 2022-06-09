using GeneralTelegramBot.Models;
using Microsoft.EntityFrameworkCore;
using TelegramMessage = Telegram.Bot.Types.Message;

namespace GeneralTelegramBot.Repository;

public class DbRepository
{
    private readonly GeneralTelegramBotDbContext dbContext;

    public DbRepository(GeneralTelegramBotDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<User> CreateUser(TelegramMessage message)
    {
        var user = new User
        {
            UserName = message.From.FirstName,
            UserSurname = message.From.LastName,
            UserLogin = message.From.Username
        };

        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<User> FindUserByUsername(string username)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.UserLogin == username);
        return user;
    }
}