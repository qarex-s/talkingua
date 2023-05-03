using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalkingUADev.Areas.Identity.Data;
using TalkingUADev.Data;
using TalkingUADev.Models;

namespace TalkingUADev.Controllers
{
    public class StoryController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<UserApp> _userManager;
        public StoryController( ApplicationDbContext context, UserManager<UserApp> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {

            return View();
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreatingStory()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatingStory(string Image)
        {
            var mainUser = await _userManager.GetUserAsync(User);
            if (!_context.listUserStories.Any(x => x.UserId == mainUser.Id)) 
            {
                ListUserStory newlistStory = new ListUserStory();
                newlistStory.UserOfStore = mainUser;
                newlistStory.UserId = mainUser.Id;
                await _context.listUserStories.AddAsync(newlistStory);
                await _context.SaveChangesAsync();
            }
                var listStories = await _context.listUserStories.Where(x => x.UserId == mainUser.Id).FirstAsync();
                UserStory userStory = new UserStory();
                userStory.ImageStory = Image;
                userStory.ListUserStoryId = listStories.Id;
                userStory.listUserStory = listStories;
                await _context.stories.AddAsync(userStory);
                await _context.SaveChangesAsync();

            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewStories(string UserId)
        {
            var someUser = await _userManager.Users.Where(x=>x.Id == UserId).FirstOrDefaultAsync();
            var allStorysUser = await _context.stories.Where(x => x.listUserStory.UserId == UserId).ToListAsync();
            if(_context.watchedStories.Any(x=> allStorysUser.Select(x=>x.Id).Contains(x.StoriesId) && x.UserId == UserId){

            }
            return View(allStorysUser);
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewStory(string UserId)
        {
            var someUser = await _userManager.Users.Where(x => x.Id == UserId).FirstOrDefaultAsync();
            var allStorysUser = await _context.stories.Where(x => x.listUserStory.UserId == UserId).ToListAsync();
            if (_context.watchedStories.Any(x => allStorysUser.Select(x => x.Id).Contains(x.StoriesId) && x.UserId == UserId){

            }
            return View(allStorysUser);
        }
    }
}
