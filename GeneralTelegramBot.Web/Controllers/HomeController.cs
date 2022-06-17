using GeneralTelegramBot.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace GeneralTelegramBot.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IUnitOfWork unitOfWork;

        public HomeController(IUnitOfWork unitOfWork, ILogger<HomeController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public IActionResult Message()
        {
            return View();
        }

        public IActionResult Photo()
        {
            var photoList = unitOfWork.PhotoRepository.GetAll();
            return View(photoList);
        }
    }
}