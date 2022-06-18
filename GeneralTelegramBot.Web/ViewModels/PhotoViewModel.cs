using GeneralTelegramBot.DataAccess.Models;

namespace GeneralTelegramBot.Web.ViewModels;

public class PhotoViewModel
{
    public List<Tuple<string, Photo>> PhotoDictionary { get; set; } = new List<Tuple<string, Photo>>();

    public string SearchString { get; set; } = null!;
}