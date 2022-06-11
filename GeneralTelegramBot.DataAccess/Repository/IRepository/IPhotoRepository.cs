using GeneralTelegramBot.DataAccess.Models;

namespace GeneralTelegramBot.DataAccess.Repository.IRepository;

public interface IPhotoRepository
{
    IEnumerable<Photo> GetPhotos();
    Photo GetPhotoByUsername(string userName);
    void InsertPhoto(Photo photo);
    void DeletePhoto(int photoId);
    void UpdatePhoto(Photo photo);
}