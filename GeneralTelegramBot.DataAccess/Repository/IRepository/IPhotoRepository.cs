using GeneralTelegramBot.DataAccess.Models;

namespace GeneralTelegramBot.DataAccess.Repository.IRepository;

public interface IPhotoRepository : IRepository<Photo>
{
    void UpdatePhoto(Photo photo);
}