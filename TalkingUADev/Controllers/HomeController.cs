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

        public IActionResult Index()
        {
            var followedUsers = _context.followUsers.Where(x=>x.UserId == _userManager.GetUserId(User) && x.isFollowed).Select(x=>x.FollowerId).ToList();
            List<UserPost> foolowedUsersPost = _context.Posts.Where(x => followedUsers.Contains(x.UserAppId)).OrderBy(x=>x.DateOfCreatingPost).ToList();
            return View(foolowedUsersPost);
        }
        [Authorize]
        public async Task<IActionResult> ProfileAsync()
        {
            UserApp _user = _context.Users.Where(x=>x.Id ==  _userManager.GetUserId(User)).FirstOrDefault();
            List < UserPost> _userPosts = _context.Posts.Where(x=>x.UserAppId == _user.Id.ToString()).ToList();
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
        public async Task<IActionResult> GetPublication(Guid?Id)
        {
            var userPost = await _context.Posts.FirstOrDefaultAsync(x => x.UserPostId == Id);

            if (userPost == null)
            {
                return NotFound();
            }
            else
            {
                UtilPostLike postLike = new UtilPostLike();
                postLike.userPost = await _context.Posts.FirstOrDefaultAsync(x => x.UserPostId == Id);
                postLike.userLike = _context.likesUsers.Where(x => x.PostId == Id.ToString() && x.isLiked).ToList();
                return View(postLike);

            }

        }
        public IActionResult Privacy()
        {
            return View();
        }



        /*

        //NEW STYLE NEWSPAGE

        public IActionResult IndexDev()
        {
            var followedUsers = _context.followUsers.Where(x => x.UserId == _userManager.GetUserId(User) && x.isFollowed).Select(x => x.FollowerId).ToList();
            List<UserPost> foolowedUsersPost = _context.Posts.Where(x => followedUsers.Contains(x.UserAppId)).OrderBy(x => x.DateOfCreatingPost).ToList();
            return View(foolowedUsersPost);
        }

        //NEW STYLE PROFILE
        [Authorize]
        public async Task<IActionResult> ProfileDevAsync()
        {
            UserApp _user = _context.Users.Where(x => x.Id == _userManager.GetUserId(User)).FirstOrDefault();
            List<UserPost> _userPosts = _context.Posts.Where(x => x.UserAppId == _user.Id.ToString()).ToList();
            _user.CountPosts = _userPosts.Count;
            _user.posts = _userPosts;
            await _userManager.UpdateAsync(_user);
            UtilUserPost utilUserAndPost = new UtilUserPost();

            utilUserAndPost.SetUserAppUtil(_user);
            utilUserAndPost.SetUserPostsUtil(_userPosts);

            return View(utilUserAndPost);
        }

        */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}