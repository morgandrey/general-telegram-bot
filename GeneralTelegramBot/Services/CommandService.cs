using GeneralTelegramBot.Commands;
using GeneralTelegramBot.Contracts;
using GeneralTelegramBot.DataAccess.Repository.IRepository;

namespace GeneralTelegramBot.Services
{
    public class CommandService : ICommandService
    {
        private readonly List<TelegramCommand> commands;
        private readonly IUnitOfWork unitOfWork;

        public CommandService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            commands = new List<TelegramCommand>
            {
                new AnecdoteCommand(),
                new AudioAnecdoteCommand(),
                new AudioCommand(),
                new HealthCheckCommand(),
                new HelpCommand(),
                new RandomCommand(),
                new SaveMessageCommand(unitOfWork),
                new SavePhotoCommand(unitOfWork)
            };
        }

        public List<TelegramCommand> Get() => commands;
    }
}