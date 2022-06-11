using GeneralTelegramBot.DataAccess.Data;
using GeneralTelegramBot.DataAccess.Models;
using GeneralTelegramBot.DataAccess.Repository.IRepository;

namespace GeneralTelegramBot.DataAccess.Repository;

public class PhotoRepository : IPhotoRepository
{
    private readonly GeneralTelegramBotDbContext dbContext;

    public PhotoRepository(GeneralTelegramBotDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IEnumerable<Photo> GetPhotos()
    {
        return dbContext.Photos.ToList();
    }

    public Photo GetPhotoByUsername(string userName)
    {
        throw new NotImplementedException();
    }

    public void InsertPhoto(Photo photo)
    {
        dbContext.Photos.Add(photo);
    }

    public void DeletePhoto(int photoId)
    {
        throw new NotImplementedException();
    }

    public void UpdatePhoto(Photo photo)
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        dbContext.SaveChanges();
    }
}