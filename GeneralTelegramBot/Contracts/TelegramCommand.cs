using Telegram.Bot;
using Telegram.Bot.Types;

namespace GeneralTelegramBot.Contracts
{
    public abstract class TelegramCommand
    {
        public abstract string Name { get; }

        public abstract Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken);

        public abstract bool Contains(Message message);
    }
}