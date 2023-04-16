using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TalkingUADev.Areas.Identity.Data;
using TalkingUADev.Data;
using TalkingUADev.Models;

namespace TalkingUADev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context;
        private UserManager<UserApp> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<UserApp> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Profile()
        {
            UserApp _user = _context.Users.Where(x=>x.Id ==  _userManager.GetUserId(User)).FirstOrDefault();
            return View(_user);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}