using Telegram.Bot;
using Telegram.Bot.Types;

namespace GeneralTelegramBot.Commands;

public static class RandomCommand
{
    private const string RandomImageUrl = "https://picsum.photos/800";

    public static async Task Execute(ITelegramBotClient botClient, Message message)
    {
        using var client = new HttpClient();
        using var response = await client.GetAsync(RandomImageUrl);
        await botClient.SendPhotoAsync(message.Chat.Id, response.Content.ReadAsStream(), "Random image");
    }
}