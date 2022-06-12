using GeneralTelegramBot.DataAccess.Data;
using GeneralTelegramBot.DataAccess.Models;
using GeneralTelegramBot.DataAccess.Repository.IRepository;

namespace GeneralTelegramBot.DataAccess.Repository;

public class PhotoRepository : Repository<Photo>, IPhotoRepository
{
    private readonly GeneralTelegramBotDbContext dbContext;

    public PhotoRepository(GeneralTelegramBotDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }
    public Photo GetPhotoByUsername(string userName)
    {
        throw new NotImplementedException();
    }

    public void UpdatePhoto(Photo photo)
    {
        throw new NotImplementedException();
    }
}