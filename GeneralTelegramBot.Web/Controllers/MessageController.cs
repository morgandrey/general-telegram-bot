using GeneralTelegramBot.DataAccess.Repository.IRepository;
using GeneralTelegramBot.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GeneralTelegramBot.Web.Controllers;

public class MessageController : Controller
{
    private readonly ILogger<MessageController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public MessageController(IUnitOfWork unitOfWork, ILogger<MessageController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index(string searchString)
    {
        var messages = _unitOfWork.MessageRepository.GetAll(includeProperties: "MessageUser").ToList();
        
        _logger.LogInformation("Messages are loaded successfully!");

        if (!string.IsNullOrEmpty(searchString))
        {
            var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.UserLogin == searchString);
            if (user != null)
            {
                messages = messages.Where(s => s.SaveUserId == user.UserId).ToList();
            }
        }

        var messageViewModel = new MessageViewModel
        {
            Messages = messages
        };
        
        return View(messageViewModel);
    }
}