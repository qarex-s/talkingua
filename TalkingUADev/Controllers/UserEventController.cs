using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
            await _context.Posts.AddAsync(tempUserPost);
            await _context.SaveChangesAsync();
            someUser.posts.Add(tempUserPost);
            someUser.CountPosts = _context.Posts.Count();
            await _user.UpdateAsync(someUser);

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Authorize]
        public IActionResult SearchUsers()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SearchUsers(string UserName)
        {
            var user = await _user.Users.Where(x => x.Name == UserName).FirstOrDefaultAsync();
            if (user != null)
            {
                return View(user);
            }
            return  View();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewProfileUser(string userId)
        {
            UtilUserPost utilUserPost = new UtilUserPost();
            var UserGet = await _user.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            if(UserGet != null)
            {
                utilUserPost.SetAll(UserGet,
            _context.Posts.Where(x => x.UserAppId == userId).ToList());

                var FollowerOrNot = await _context.followUsers.Where(x => x.FollowerId == userId && x.UserId == _user.GetUserId(User)).FirstOrDefaultAsync();
                if (FollowerOrNot != null)
                    utilUserPost.SetUserIsFollowed(FollowerOrNot.isFollowed);
            }
            else
            {
                //HERE WILL BE PAGE OF ERRORS
                return RedirectToAction("Privacy");
            }
            
            

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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LikePost(string postId)
        {
            var LikeUserTemp = await _context.likesUsers.Where(x=>x.PostId == postId && x.UserId == _user.GetUserId(User)).FirstOrDefaultAsync();
            if(LikeUserTemp == null)
            {
                LikeUserTemp = new LikeUser();
                LikeUserTemp.UserId = _user.GetUserId(User);
                LikeUserTemp.PostId = postId;
                LikeUserTemp.isLiked = true;

                await _context.likesUsers.AddAsync(LikeUserTemp);

            }
            else 
            {
                if (LikeUserTemp.isLiked)
                    LikeUserTemp.isLiked = false;
                else
                    LikeUserTemp.isLiked = true;

            }
            await _context.SaveChangesAsync();

            var post = await _context.Posts.Where(x=>x.UserPostId.ToString() == postId).FirstOrDefaultAsync();
            if(post !=null)
                post.Likes = _context.likesUsers.Where(x=>x.PostId == postId && x.isLiked == true).ToList().Count();
            else
                return RedirectToAction("Privacy","Home");//PAGE ERRORS
            await _context.SaveChangesAsync();

            return RedirectToAction("GetPublication","Home",new { Id = postId});
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewLikedPost()
        {
            
            var user = await _user.GetUserAsync(User);
            var postsId = _context.likesUsers.Where(x => x.UserId == user.Id && x.isLiked).Select(x=>x.PostId).ToList();
             
            return View(_context.Posts.Where(x => postsId.Contains(x.UserPostId.ToString())).ToList());
        }



    }


}
