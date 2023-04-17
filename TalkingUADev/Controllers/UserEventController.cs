using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TalkingUADev.Areas.Identity.Data;
using TalkingUADev.Data;
using TalkingUADev.Models;
using TalkingUADev.ViewModels;

namespace TalkingUADev.Controllers
{
    public class UserEventController : Controller
    {
        private UserManager<UserApp> _user;
        private ApplicationDbContext _context;
        public UserEventController(ApplicationDbContext context, UserManager<UserApp> user)
        {
            _context = context;
            _user = user;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public IActionResult AddPublication()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPublication(ModelPost post)
        {
            var tempUserPost = new UserPost
            {

                Name = post.Name,
                Desc = post.Desc,
                Image = post.Image,
                Likes = post.Likes,
                Position = post.Position,
                Tags = post.Tags,
                UserAppId = _user.GetUserId(User)

            };
            UserApp someUser = await _user.GetUserAsync(User);//thx this meth we can change some prop of UserApp which extends IdentityUser
            _context.Posts.Add(tempUserPost);
            _context.SaveChangesAsync();
            someUser.posts.Add(tempUserPost);
            _user.UpdateAsync(someUser);
           
            return RedirectToAction("Index","Home");
        }
    }
}
