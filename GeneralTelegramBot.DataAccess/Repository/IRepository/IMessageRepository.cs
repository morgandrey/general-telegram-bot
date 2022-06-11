using GeneralTelegramBot.DataAccess.Models;

namespace GeneralTelegramBot.DataAccess.Repository.IRepository;

public interface IMessageRepository
{
    Message GetMessageByUserName(string userName);
    void InsertMessage(Message message);
}