using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TalkingUADev.Areas.Identity.Data;
using TalkingUADev.Data;
using TalkingUADev.Models;
using TalkingUADev.Util;
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
            someUser.CountPosts = _context.Posts.Count();
            _user.UpdateAsync(someUser);

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SearchUsers()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SearchUsers(string UserName)
        {
            var user = _user.Users.Where(x => x.Name == UserName).FirstOrDefault();
            if (user != null)
            {
                return View(user);
            }
            return View();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewProfileUser(string userId)
        {
            UtilUserPost utilUserPost = new UtilUserPost();
            utilUserPost.SetAll(_user.Users.Where(x => x.Id == userId).FirstOrDefault(),
            _context.Posts.Where(x => x.UserAppId == userId).ToList());
            var FollowerOrNot = _context.followUsers.Where(x => x.FollowerId == userId && x.UserId == _user.GetUserId(User)).FirstOrDefault();
            if (FollowerOrNot != null)
                utilUserPost.SetUserIsFollowed(FollowerOrNot.isFollowed);

            return View(utilUserPost);
        }
        [HttpGet]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> FollowToUser(string userId)
        {

            var user = _user.Users.FirstOrDefault(x => x.Id == userId);
            var contextFollowUser = _context.followUsers.Where(x => x.FollowerId == userId && x.UserId == _user.GetUserId(User)).FirstOrDefault();
            FollowUser followToUserObj = new FollowUser();
            followToUserObj.UserId = _user.GetUserId(User);
            followToUserObj.FollowerId =  userId;
            if (contextFollowUser != null)
            {
                if (!contextFollowUser.isFollowed)
                    contextFollowUser.isFollowed = true;
                else
                    contextFollowUser.isFollowed = false;

                followToUserObj.DateOfFollowing = DateTime.Now;

                contextFollowUser = followToUserObj;
            }
            else
            {
                followToUserObj.isFollowed = true;
                followToUserObj.DateOfFollowing = DateTime.Now;
                await _context.followUsers.AddAsync(followToUserObj);
            }
            await _context.SaveChangesAsync();

            UserApp MainUser = await _user.GetUserAsync(User);
            MainUser.CountFollows = _context.followUsers.Where(_x => _x.UserId == MainUser.Id && _x.isFollowed == true).Count();

            user.CountSubs = _context.followUsers.Where(_x => _x.FollowerId == userId && _x.isFollowed==true ).Count();
            await _user.UpdateAsync(user);
            await _user.UpdateAsync(MainUser);

            return RedirectToAction("ViewProfileUser", "UserEvent", new { userId });
        }
    }
}
