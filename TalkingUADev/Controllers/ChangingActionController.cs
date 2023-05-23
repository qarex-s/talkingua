using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalkingUADev.Areas.Identity.Data;
using TalkingUADev.Data;
using TalkingUADev.Models;
using TalkingUADev.ViewModels;

namespace TalkingUADev.Controllers
{
    public class ChangingActionController : Controller
    {
        private  SignInManager<UserApp> _signInManager;
        private ApplicationDbContext _context;
        private UserManager<UserApp> _userManager;
        private IWebHostEnvironment _webHostEnvironment;
        public ChangingActionController(ApplicationDbContext context, UserManager<UserApp> userManager, IWebHostEnvironment webHostEnvironment, SignInManager<UserApp> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _signInManager = signInManager;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPublicationForEdit()
        {
               var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var userPosts = await _context.Posts.Where(x => x.UserAppId == user.Id).OrderByDescending(x=>x.DateOfCreatingPost).ToListAsync();
                return View(userPosts);
            }
            return BadRequest("User wasn't find");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> PartialGetPublication()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var userPosts = await _context.Posts.Where(x => x.UserAppId == user.Id).ToListAsync();
                return PartialView("_PartialGetPublication",userPosts);
            }
            return BadRequest("User wasn't find");
        }




        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ChangePublication(Guid postId)
        {

            var choosedPost = await _context.Posts.Where(x => x.UserPostId == postId).FirstAsync();
            var user = await _userManager.GetUserAsync(User);
            if(choosedPost.UserAppId == user.Id)
            {
                return PartialView("_PartialChangePublication", choosedPost);
            }
            return RedirectToAction("Index","Home"); 
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePublication(Guid UserPostId, string Name, string Desc, string Position)
        {
            var choosedPost = await _context.Posts.Where(x => x.UserPostId == UserPostId).FirstOrDefaultAsync();

            if (choosedPost != null)
            {
                choosedPost.Name = Name;
                choosedPost.Desc = Desc;
                choosedPost.Position = Position;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("GetPublicationForEdit");
        }

       

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeletePublication(Guid postId)
        {
            var choosedPost = await _context.Posts.Where(x => x.UserPostId == postId).FirstAsync();
            if (choosedPost != null)
            { 
                _context.Posts.Remove(choosedPost);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("PartialGetPublication");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProfileForEdit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                return View(user);
            }
            return BadRequest("User wasn't find");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SetProfileNewProp(string UserName, string oldPass, string newPass, string confirmPass, IFormFile imageUser)
        {
            var user = await _userManager.GetUserAsync(User);
            if(oldPass != null && newPass != null && oldPass!="" && newPass!="" && confirmPass == newPass)
            {
                await _signInManager.UserManager.ChangePasswordAsync(user, oldPass, newPass);

                //var res = await _signInManager.UserManager.ChangePasswordAsync(user, oldPass, newPass);

                //if (res.Succeeded)
                //{
                //    await _signInManager.SignInAsync(user, isPersistent: false);
                //}
                //else
                //{
                //    return BadRequest("pass");
            //}
            }
            if(UserName!=null || UserName != "")
            {
                user.Name = UserName;
            }
            if (imageUser != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string SubDirPath = $"UserPostBy{user.Email}";
                DirectoryInfo directoryInfo = new DirectoryInfo(wwwRootPath + "/UsersAvatars");
                if (directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }

                directoryInfo.CreateSubdirectory(SubDirPath);
                string fileName = Path.GetFileNameWithoutExtension(imageUser.FileName);
                string extension = Path.GetExtension(imageUser.FileName);
                user.ImageTitle = user.Email + fileName + user.Name + extension;
                string path = Path.Combine(wwwRootPath + "/UsersAvatars/" + SubDirPath + "/" + user.ImageTitle);
                user.ImageFile = imageUser;
                using (var FileStream = new FileStream(path, FileMode.Create))
                {
                    await user.ImageFile.CopyToAsync(FileStream);
                }
                //tempUserPost.ImageTitle = post.ImageTitle;
                
            }
            await _userManager.UpdateAsync(user);
            await _signInManager.UserManager.UpdateAsync(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetPublicationForEdit");
        }




        public IActionResult ChanchingComments()
        {
            return View();
        }
        public IActionResult ChanchingProfile()
        {
            return View();
        }
    }
}
