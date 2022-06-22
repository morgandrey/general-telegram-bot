using GeneralTelegramBot.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GeneralTelegramBot.Commands;

public class RandomCommand : TelegramCommand
{
    private const string RandomImageUrl = "https://thiscatdoesnotexist.com/";
    private const string PhotoCaption = "Random image";

    public override string Name => "/random";

    public override async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        using var client = new HttpClient();
        using var response = await client.GetAsync(RandomImageUrl, cancellationToken);
        await botClient.SendPhotoAsync(message.Chat.Id,
            response.Content.ReadAsStream(),
            PhotoCaption,
            disableNotification: true,
            cancellationToken: cancellationToken);
    }

    public override bool Contains(Message message)
    {
        return message.Text != null &&
               message.Text.Split(' ', '@')[0].Contains(Name);
    }
}