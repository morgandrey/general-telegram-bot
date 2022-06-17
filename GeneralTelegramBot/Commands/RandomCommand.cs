using Telegram.Bot;
using Telegram.Bot.Types;

namespace GeneralTelegramBot.Commands;

public static class RandomCommand
{
    private const string RandomImageUrl = "https://thiscatdoesnotexist.com/";

    public static async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        using var client = new HttpClient();
        using var response = await client.GetAsync(RandomImageUrl, cancellationToken);
        await botClient.SendPhotoAsync(message.Chat.Id,
            response.Content.ReadAsStream(),
            "Random image",
            cancellationToken: cancellationToken);
    }
}