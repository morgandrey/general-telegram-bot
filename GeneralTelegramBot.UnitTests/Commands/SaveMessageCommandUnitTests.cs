using GeneralTelegramBot.Commands;
using GeneralTelegramBot.DataAccess.Repository.IRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using TelegramMessage = Telegram.Bot.Types.Message;

namespace GeneralTelegramBot.UnitTests.Commands
{
    [TestClass]
    public class SaveMessageCommandUnitTests
    {
        private readonly string commandName = "/savem";
        private readonly Mock<IUnitOfWork> unitOfWork = new Mock<IUnitOfWork>();

        [TestMethod]
        public void ExecuteContains_ShouldReturnTrue_WhenSaveMessage()
        {
            // Arrange
            var telegramMessage = CreateReplyTelegramMessage();

            var saveMessageCommand = new SaveMessageCommand(unitOfWork.Object);

            // Act
            var result = saveMessageCommand.Contains(telegramMessage);

            // Assert
            Assert.IsTrue(result);
        }

        private TelegramMessage CreateReplyTelegramMessage()
        {
            return new TelegramMessage
            {
                Text = commandName,
                ReplyToMessage = new TelegramMessage
                {
                    Text = "text"
                }
            };
        }
    }
}
