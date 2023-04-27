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
                UserAppId = _user.GetUserId(User),
                user = await _user.GetUserAsync(User)
            };
            
            UserApp someUser = await _user.GetUserAsync(User);//thx this meth we can change some prop of UserApp which extends IdentityUser
            if(someUser.Image == null)
            {
                return BadRequest();
            }
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
        public async Task<IActionResult> PartialSearching(string UserName)
        {
            var users = await _user.Users.Where(x => x.Name.Contains(UserName)).ToListAsync();
            if (users != null)
            {
                return PartialView(users);
            }
            return BadRequest();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SearchUsers(string UserName)
        {
            var users = await _user.Users.Where(x => x.Name.Contains(UserName)).ToListAsync();
            if (users != null)
            {
                return PartialView("_PartialSearching", users);
            }
            return BadRequest();
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
                await _context.Posts.Where(x => x.UserAppId == userId)
                .OrderByDescending(x=>x.DateOfCreatingPost)
                .ToListAsync());

                var FollowerOrNot = await _context.followUsers.Where(x => x.FollowerId == userId && x.UserId == _user.GetUserId(User)).FirstOrDefaultAsync();
                if (FollowerOrNot != null)
                    utilUserPost.SetUserIsFollowed(FollowerOrNot.isFollowed);
            }
            else
            {
                //HERE WILL BE PAGE OF ERRORS
                return BadRequest();
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LikePost(string postId)
        {
            var LikeUserTemp = await _context.likesUsers.Where(x => x.PostId == postId && x.UserId == _user.GetUserId(User)).FirstOrDefaultAsync();
            if (LikeUserTemp == null)
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

            var post = await _context.Posts.Where(x => x.UserPostId.ToString() == postId).FirstOrDefaultAsync();
            if (post != null)
                post.Likes = _context.likesUsers.Where(x => x.PostId == postId && x.isLiked == true).ToList().Count();
            else
                return RedirectToAction("Privacy", "Home");//PAGE ERRORS
            await _context.SaveChangesAsync();

            return PartialView("_PartialCountLikes", post);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMySubscribers()
        {
            UserApp user = await _user.GetUserAsync(User);
            
            var subs =  _context.followUsers.Where(x=>x.FollowerId == user.Id && x.isFollowed).Select(x=>x.UserId);
            if (subs != null)
            {
                return View(_user.Users.Where(x => subs.Contains(x.Id)).ToList());
            }
            return View();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyFollowers()
        {
            UserApp user = await _user.GetUserAsync(User);

            var followers = _context.followUsers.Where(x => x.UserId == user.Id && x.isFollowed).Select(x => x.FollowerId);
            if (followers != null)
            {
                return View(_user.Users.Where(x => followers.Contains(x.Id)).ToList());
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> WriteComment(Guid postId,string textMessage)
        {
            UserComment userComment = new UserComment();
            var user = await _user.GetUserAsync(User);
            var choosedPost = await _context.Posts.Where(x => x.UserPostId == postId).FirstAsync();
            if (user!=null)
            {
                userComment.FromUserId = user.Id;
                userComment.ToPostId = postId;
                userComment.TextMessage = textMessage;
                userComment.userApp = user;
                userComment.post = choosedPost;
                string formatDate = userComment.DateOfCreatingComment.ToString("dd/MM/yy/HH/mm");
                
                await _context.commentsUsers.AddAsync(userComment);

                await _context.SaveChangesAsync();
                return RedirectToAction("GetPublication","Home",new { Id = postId});

            }
            else
            {
                return RedirectToAction("Privacy", "Home");
            }
        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewLikedPost()
        {
            
            var user = await _user.GetUserAsync(User);
            var postsId =await  _context.likesUsers.Where(x => x.UserId == user.Id && x.isLiked).Select(x=>x.PostId).ToListAsync();
             
            return View(await _context.Posts.Where(x => postsId.Contains(x.UserPostId.ToString())).OrderByDescending(x=>x.DateOfCreatingPost).ToListAsync());
        }



    }


}
