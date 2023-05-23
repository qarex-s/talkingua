using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TalkingUADev.Areas.Identity.Data;
using TalkingUADev.Data;
using TalkingUADev.Models;
using TalkingUADev.Util;

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




        public async Task<IActionResult> Index()
        {
            UtilAllPostProp uAllPost = new UtilAllPostProp();
            var user = await _userManager.GetUserAsync(User);

            uAllPost.user = user;

            if (user == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            var followedUsers = await _context.followUsers
                .Where(x => x.UserId == _userManager
                .GetUserId(User) && x.isFollowed)
                .Select(x => x.FollowerId)
                .ToListAsync();
            if(followedUsers.Count < 1)
            {
                followedUsers = await _context.Users.Where(x => x.City.Contains(user.City)).Select(x=>x.Id).ToListAsync();
            }

            var ActiveStories = await _context.stories
                .Where(x => x.isActiveStory && followedUsers
                .Contains(x.listUserStory.UserId))
                .Select(x => x.Id)
                .ToListAsync();

            var NewFollowUsers = await _context.stories
                .Where(x => ActiveStories.Contains(x.Id))
                .Include(x => x.listUserStory)
                .Select(x => x.listUserStory)
                .Where(x => followedUsers
                .Contains(x.UserId))
                .Select(x => x.UserOfStore)
                .ToListAsync();

            uAllPost.followUsers = NewFollowUsers;
            uAllPost.userPosts = await _context.Posts
                .Where(x => followedUsers
                .Contains(x.UserAppId))
                .Include(x => x.user)
                .OrderByDescending(x => x.DateOfCreatingPost)
                .ToListAsync();

            uAllPost.userComments = await _context.commentsUsers
                .Where(x => uAllPost.userPosts
                .Select(x => x.UserPostId)
                .Contains(x.ToPostId))
                .Include(x => x.userApp)
                .OrderByDescending(x => x.DateOfCreatingComment)
                .ToListAsync();

            return View(uAllPost);
        }




        [Authorize]
        public async Task<IActionResult> ProfileAsync()
        {
            UserApp _user = _context.Users.Where(x => x.Id == _userManager.GetUserId(User)).First();
            List<UserPost> _userPosts = await _context.Posts
                .Where(x => x.UserAppId == _user.Id.ToString())
                .OrderByDescending(x => x.DateOfCreatingPost)
                .ToListAsync();
            _user.CountPosts = _userPosts.Count;
            _user.posts = _userPosts;
            await _userManager.UpdateAsync(_user);

            UtilUserPost utilUserAndPost = new UtilUserPost();
            utilUserAndPost.SetUserAppUtil(_user);
            utilUserAndPost.SetUserPostsUtil(_userPosts);

            return View(utilUserAndPost);
        }




        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPublication(Guid? Id)
        {
            var userPost = await _context.Posts
                .FirstOrDefaultAsync(x => x.UserPostId == Id);

            var AuthUser = await _userManager
                .GetUserAsync(User);
            //temporaryCRINGE
            var userOfPost = userPost.user;
            if (userPost == null)
            {
                return NotFound();
            }
            else
            {
                UtilPostLike postLike = new UtilPostLike();
                postLike.userPost = await _context.Posts
                    .Include(x => x.user)
                    .FirstAsync(x => x.UserPostId == Id);

                postLike.userLike = await _context.likesUsers
                    .Where(x => x.PostId == Id.ToString() && x.isLiked)
                    .ToListAsync();
                List<UserComment> postCommented = await _context.commentsUsers
                    .Where(x => x.ToPostId == Id)
                    .ToListAsync();

                postLike.postComments = postCommented;
                List<string> usersId = postCommented.Select(x => x.FromUserId).ToList();
                postLike.usersComments = await _userManager.Users
                    .Where(x => usersId.Contains(x.Id))
                    .ToListAsync();
                postLike.postComments = postLike.postComments.OrderByDescending(x => x.DateOfCreatingComment).ToList();
                return View(postLike);

            }

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