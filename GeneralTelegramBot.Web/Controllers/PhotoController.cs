using GeneralTelegramBot.DataAccess.Models;
using GeneralTelegramBot.DataAccess.Repository.IRepository;
using GeneralTelegramBot.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GeneralTelegramBot.Web.Controllers
{
    public class PhotoController : Controller
    {
        private readonly ILogger<PhotoController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public PhotoController(IUnitOfWork unitOfWork, ILogger<PhotoController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(string searchString)
        {
            var photoViewModel = new PhotoViewModel();
            var photoList = _unitOfWork.PhotoRepository.GetAll();
            _logger.LogInformation("Photos are loaded successfully!");

            if (!string.IsNullOrEmpty(searchString))
            {
                var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.UserLogin == searchString);
                if (user != null && user.Photos.Any())
                {
                    photoList = photoList.Where(s => s.UserId == user.UserId);
                }
            }

            foreach (var photo in photoList)
            {
                var imgBase64Data = Convert.ToBase64String(photo.PhotoSource);
                var imgDataURL = $"data:image/png;base64,{imgBase64Data}";
                photoViewModel.PhotoDictionary.Add(new Tuple<string, Photo>(imgDataURL, photo));
            }
            
            return View(photoViewModel);
        }
    }
}