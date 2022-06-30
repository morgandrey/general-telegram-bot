using System.Linq.Expressions;
using GeneralTelegramBot.DataAccess.Models;
using GeneralTelegramBot.DataAccess.Repository.IRepository;
using GeneralTelegramBot.Web.Controllers;
using GeneralTelegramBot.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Assert = Xunit.Assert;

namespace GeneralTelegramBot.Web.UnitTests;

[TestClass]
public class PhotoControllerUnitTests
{
    private const string searchString = "SearchString";
    
    [TestMethod]
    public void Index_ReturnsAViewResult_WithAListOfPhotos()
    {
        // Arrange
        var unitOfWork = new Mock<IUnitOfWork>();
        var logger = new Mock<ILogger<PhotoController>>();
        unitOfWork.Setup(repo => repo.PhotoRepository.GetAll(
                null,
                "User"))
            .Returns(GetPhotos())
            .Verifiable();
        var controller = new PhotoController(unitOfWork.Object, logger.Object);

        // Act
        var result = controller.Index(string.Empty);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PhotoViewModel>(
            viewResult.Model);
        Assert.Equal(2, model.PhotoDictionary.Count);
        Mock.Verify();
    }
    
    [TestMethod]
    public void Index_ReturnsAViewResult_WithAListOfPhotosWithSearchString()
    {
        // Arrange
        var unitOfWork = new Mock<IUnitOfWork>();
        var logger = new Mock<ILogger<PhotoController>>();
        unitOfWork.Setup(repo => repo.PhotoRepository.GetAll(
                null,
                "User"))
            .Returns(GetPhotos())
            .Verifiable();
        unitOfWork.Setup(repo => repo.UserRepository.GetFirstOrDefault(
            x => x.UserLogin == searchString,
            null, 
            true))
            .Returns(GetUser())
            .Verifiable();
        var controller = new PhotoController(unitOfWork.Object, logger.Object);

        // Act
        var result = controller.Index(searchString);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PhotoViewModel>(
            viewResult.Model);
        Assert.Single(model.PhotoDictionary);
        Mock.Verify();
    }

    private static User GetUser()
    {
        return new User
        {
            UserId = 1
        };
    }

    private static IEnumerable<Photo> GetPhotos()
    {
        return new List<Photo>
        {
            new Photo
            {
                PhotoId = 1,
                PhotoSource = Array.Empty<byte>(),
                PhotoCreationDate = new DateTime(2022, 01, 01),
                UserId = 1
            },
            new Photo
            {
                PhotoId = 2,
                PhotoSource = Array.Empty<byte>(),
                PhotoCreationDate = new DateTime(2022, 02, 01)
            }
        };
    }
}