using GeneralTelegramBot.Contracts;
using GeneralTelegramBot.DataAccess.Models;
using GeneralTelegramBot.DataAccess.Repository.IRepository;
using Telegram.Bot;
using TelegramMessage = Telegram.Bot.Types.Message;

namespace GeneralTelegramBot.Commands;

public class SaveMessageCommand : TelegramCommand
{
    private readonly IUnitOfWork unitOfWork;

    public override string Name => "/savem";

    public SaveMessageCommand(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public override async Task Execute(ITelegramBotClient botClient, TelegramMessage telegramMessage, CancellationToken cancellationToken)
    {
        var chatId = telegramMessage.Chat.Id;

        var saveMessageUser = unitOfWork.UserRepository.TryAddUser(new User
        {
            UserName = telegramMessage.From.FirstName,
            UserSurname = telegramMessage.From.LastName,
            UserLogin = telegramMessage.From.Username
        });
        unitOfWork.Save();

        var messageUser = unitOfWork.UserRepository.TryAddUser(new User
        {
            UserName = telegramMessage.ReplyToMessage.From.FirstName,
            UserSurname = telegramMessage.ReplyToMessage.From.LastName,
            UserLogin = telegramMessage.ReplyToMessage.From.Username
        });
        unitOfWork.Save();

        var message = new Message
        {
            MessageContent = telegramMessage.ReplyToMessage.Text,
            MessageCreationDate = DateTime.Now,
            MessageUserId = messageUser.UserId,
            SaveUserId = saveMessageUser.UserId
        };

        unitOfWork.MessageRepository.Add(message);
        unitOfWork.Save();
        await botClient.SendTextMessageAsync(chatId,
            "Message saved successfully!",
            cancellationToken: cancellationToken);
    }

    public override bool Contains(TelegramMessage message)
    {
        return message.Text != null &&
               message.ReplyToMessage?.Text != null &&
               message.Text.Split(' ', '@')[0].Contains(Name);
    }
}