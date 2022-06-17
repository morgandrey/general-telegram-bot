using GeneralTelegramBot;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types.Enums;

var bot = new TelegramBotClient(Constants.TelegramToken);

Console.WriteLine($"The bot {bot.GetMeAsync().Result.Username} is ready.");
using var cts = new CancellationTokenSource();

bot.StartReceiving(updateHandler: Handlers.HandleUpdateAsync,
    errorHandler: Handlers.HandleErrorAsync,
    receiverOptions: new ReceiverOptions()
    {
        AllowedUpdates = Array.Empty<UpdateType>()
    },
    cancellationToken: cts.Token);

Console.ReadLine();

cts.Cancel();