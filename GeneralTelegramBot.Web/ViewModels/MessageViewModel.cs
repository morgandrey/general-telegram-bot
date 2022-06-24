using GeneralTelegramBot.DataAccess.Models;

namespace GeneralTelegramBot.Web.ViewModels;

public class MessageViewModel
{
    public IEnumerable<Message> Messages { get; set; }
    public string SearchString { get; set; } = null!;
}