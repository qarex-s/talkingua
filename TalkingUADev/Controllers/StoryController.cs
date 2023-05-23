using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalkingUADev.Areas.Identity.Data;
using TalkingUADev.Data;
using TalkingUADev.Models;
using TalkingUADev.Util;
using TalkingUADev.ViewModels;

namespace TalkingUADev.Controllers
{
    public class StoryController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<UserApp> _userManager;
        private IWebHostEnvironment _webHostEnvironment;



        public StoryController(ApplicationDbContext context, UserManager<UserApp> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<IActionResult> CreatingStory(ModelStory storyImage)
        {
            if (ModelState.IsValid)
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
                var listStories = await _context.listUserStories
                .Where(x => x.UserId == mainUser.Id)
                .FirstAsync();

                UserStory userStory = new UserStory();
                userStory.ImageStory = null;
                userStory.ListUserStoryId = listStories.Id;
                userStory.listUserStory = listStories;



                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string SubDirPath = $"UserPostBy{mainUser.Email}";

                DirectoryInfo directoryInfo = new DirectoryInfo(wwwRootPath + "/UsersStoryFile");
                if (directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }

                directoryInfo.CreateSubdirectory(SubDirPath);
                string fileName = Path.GetFileNameWithoutExtension(storyImage.ImageFile.FileName);
                string extension = Path.GetExtension(storyImage.ImageFile.FileName);

                userStory.ImageTitle = mainUser.Email + fileName + mainUser + extension;
                string path = Path.Combine(wwwRootPath + "/UsersStoryFile/" + SubDirPath + "/" + userStory.ImageTitle);
                userStory.ImageFile = storyImage.ImageFile;

                using (var FileStream = new FileStream(path, FileMode.Create))
                {
                    await storyImage.ImageFile.CopyToAsync(FileStream);
                }
                var LastStories = await _context.stories
                    .Where(x => x.listUserStory.UserId == mainUser.Id)
                    .OrderByDescending(x => x.DateOfCreatingStory)
                    .FirstOrDefaultAsync();
                if (LastStories != null)
                {
                    LastStories.isActiveStory = false;
                    await _context.SaveChangesAsync();
                }
                await _context.stories.AddAsync(userStory);
                await _context.SaveChangesAsync();

                return View();

            }
            else
            {
                return View();
            }
           
        }



        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewStories(string UserId)
        {
            var mainUser = await _userManager.GetUserAsync(User);
            var someUser = await _userManager.Users.Where(x => x.Id == UserId).FirstOrDefaultAsync();
            List<UserStory> allStorysUser;

            if (await _context.stories.AnyAsync(x => x.listUserStory.UserId == UserId))
            {
                allStorysUser = await _context.stories.Where(x => x.listUserStory.UserId == UserId).ToListAsync();

            }
            else
            {
                return PartialView("_PartialListStories", null);
            }

            var someStory = allStorysUser
                .OrderByDescending(x => x.DateOfCreatingStory)
                .FirstOrDefault();

            UtilStories utilStories = new UtilStories();
            if (someStory != null)
            {
                var getTimeOfStory = DateTime.Now - someStory.DateOfCreatingStory;

                if (getTimeOfStory.TotalHours <= 24)
                {
                    utilStories.stories = allStorysUser;
                    utilStories.someStory = someStory;
                    bool watchedStories = await _context.watchedStories.AnyAsync(x => x.UserId == mainUser.Id && x.StoriesId == someStory.Id);

                    if (watchedStories)
                    {
                        WatchedStoriesByUser watchedStoriesObj = new WatchedStoriesByUser();
                        watchedStoriesObj.StoriesId = someStory.Id;
                        watchedStoriesObj.someStory = someStory;
                        watchedStoriesObj.UserId = someUser.Id;
                        watchedStoriesObj.mainUser = someUser;
                        await _context.watchedStories.AddAsync(watchedStoriesObj);
                        allStorysUser.Where(x => x.Id == someStory.Id).First().CountWathcedStory = await _context.watchedStories
                            .Where(x => x.StoriesId == someStory.Id)
                            .CountAsync() + 1;
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    allStorysUser.Where(x => x.Id == someStory.Id).First().isActiveStory = false;
                    await _context.SaveChangesAsync();

                    return PartialView("_PartialListStories", null);
                }
            }


            utilStories.followerUser = await _userManager.Users.Where(x => x.Id == UserId).FirstAsync();


            return PartialView("_PartialListStories", utilStories);
        }



        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewStory(string UserId)
        {
            var someUser = await _userManager.Users.Where(x => x.Id == UserId).FirstOrDefaultAsync();
            var allStorysUser = await _context.stories.Where(x => x.listUserStory.UserId == UserId && x.isActiveStory).ToListAsync();

            UtilStories utilStories = new UtilStories();
            utilStories.stories = allStorysUser;
            utilStories.followerUser = someUser;
            return View(utilStories);
        }



        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetStoryById(int StoryId)
        {
            var someStory = await _context.stories.Where(x => x.Id == StoryId).FirstAsync();

            if (!_context.watchedStories.Any(x => x.StoriesId == someStory.Id && x.UserId == _userManager.GetUserId(User)))
            {
                WatchedStoriesByUser watchedStory = new WatchedStoriesByUser();
                watchedStory.StoriesId = StoryId;
                watchedStory.someStory = someStory;
                watchedStory.UserId = _userManager.GetUserId(User);
                watchedStory.mainUser = await _userManager.GetUserAsync(User);

                await _context.watchedStories.AddAsync(watchedStory);
                await _context.SaveChangesAsync();

            }
            someStory.CountWathcedStory = _context.watchedStories.Where(x => x.StoriesId == StoryId).Count();
            await _context.SaveChangesAsync();
            return PartialView("_PartialStory", someStory);
        }

    }
}
