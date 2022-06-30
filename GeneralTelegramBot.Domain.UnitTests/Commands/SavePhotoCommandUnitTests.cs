using GeneralTelegramBot.Commands;
using GeneralTelegramBot.DataAccess.Repository.IRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Telegram.Bot.Types;
using TelegramMessage = Telegram.Bot.Types.Message;

namespace GeneralTelegramBot.UnitTests.Commands
{
    [TestClass]
    public class SavePhotoCommandUnitTests
    {
        private readonly string fileId = "FileId";
        private readonly string commandName = "/save";
        private readonly Mock<IUnitOfWork> unitOfWork = new Mock<IUnitOfWork>();

        [TestMethod]
        public void ExecuteContains_ShouldReturnTrue_WhenSavePhoto()
        {
            // Arrange
            var telegramMessage = CreateTelegramMessage();

            var savePhotoCommand = new SavePhotoCommand(unitOfWork.Object);

            // Act
            var result = savePhotoCommand.Contains(telegramMessage);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ExecuteContains_ShouldReturnTrue_WhenReplySavePhoto()
        {
            // Arrange
            var telegramMessage = CreateReplyTelegramMessage();

            var savePhotoCommand = new SavePhotoCommand(unitOfWork.Object);

            // Act
            var result = savePhotoCommand.Contains(telegramMessage);

            // Assert
            Assert.IsTrue(result);
        }

        private TelegramMessage CreateTelegramMessage()
        {
            return new TelegramMessage
            {
                Caption = commandName,
                Photo = new[]
                {
                    new PhotoSize
                    {
                        FileId = fileId
                    }
                }
            };
        }

        private TelegramMessage CreateReplyTelegramMessage()
        {
            return new TelegramMessage
            {
                Text = commandName,
                ReplyToMessage = new TelegramMessage
                {
                    Photo = new[]
                    {
                        new PhotoSize
                        {
                            FileId = fileId
                        }
                    }
                }

            };
        }
    }
}