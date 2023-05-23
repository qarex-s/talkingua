using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;
using TalkingUADev.Areas.Identity.Data;
using TalkingUADev.Data;
using TalkingUADev.Models;
using TalkingUADev.Util;

namespace TalkingUADev.Controllers
{
    public class ChatController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<UserApp> _userManager;
        public ChatController(ApplicationDbContext context, UserManager<UserApp> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Menu(int? RoomId)
        {
            UtilChat chat = new UtilChat();
            var MainUser = await _userManager.GetUserAsync(User);
           
            if ( _context.chats.Where(x=>x.MainUserId== MainUser.Id || x.SecondUserId == MainUser.Id).Count()== 0)
            {
                var userFollows = await _context.followUsers
                        .Where(x => x.isFollowed && x.UserId == MainUser.Id)
                        .Select(x => x.FollowerId)
                        .ToListAsync();

                chat.FriendsUser = await _userManager.Users
                    .Where(x => userFollows.Contains(x.Id))
                    .ToListAsync();

                chat.chatRooms = null;
                chat.messages = null;
                chat.mainUser = await _userManager.GetUserAsync(User);

            }
            else
            {
                try
                {
                    if (RoomId == null)
                    {
                        RoomId = _context.chats.Where(x => x.MainUserId == MainUser.Id || x.SecondUserId == MainUser.Id).Select(x => x.chatRoom).First().ChatRoomId;
                    }
                    var selectedChat = await _context.chats
                        .Where(x => x.chatRoomId == RoomId && (x.MainUserId == MainUser.Id || x.SecondUserId == MainUser.Id))
                        .Include(x => x.chatRoom)
                        .Include(x=>x.MainUser)
                        .Include(x=>x.SecondUser)
                        .FirstOrDefaultAsync();

                    if (selectedChat == null)
                    {
                        return BadRequest("selectedChat");
                    }
                    var userFollows = await _context.followUsers
                        .Where(x => x.isFollowed && x.UserId == MainUser.Id)
                        .Select(x=>x.FollowerId)
                        .ToListAsync();

                    chat.FriendsUser = await _userManager.Users
                        .Where(x => userFollows.Contains(x.Id))
                        .ToListAsync();

                    chat.ListChats = await _context.chats.Where(x => x.MainUserId == MainUser.Id || x.SecondUserId == MainUser.Id)
                        .Include(x => x.MainUser)
                        .Include(x => x.SecondUser)
                        .Include(x=>x.chatRoom)
                        .ToListAsync();
                    chat.chatRooms = chat.ListChats.Select(x => x.chatRoom).ToList();
                    chat.someChat = selectedChat;
                    chat.mainUser = await _userManager.GetUserAsync(User);
                    chat.messages = await _context.messages
                        .Where(x => x.ChatId == selectedChat.Id)
                        .Include(x => x.mainUserSender)
                        .Include(x=>x.Chat)
                        .ThenInclude(x=>x.chatRoom)
                        .ToListAsync();
                }
                catch(Exception ex)
                {

                    return BadRequest("unable to create messenger main" + ex.Message); ;
                }
                
            }



            return View(chat);

        }



        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MenuPartial(int? RoomId)
        {
            UtilChat chat = new UtilChat();
            var MainUser = await _userManager.GetUserAsync(User);

      
                try
                {
                    if (RoomId == null)
                    {
                        RoomId = _context.chats
                        .Where(x => x.MainUserId == MainUser.Id || x.SecondUserId == MainUser.Id)
                        .Select(x => x.chatRoom)
                        .First().ChatRoomId;

                    }
                    var selectedChat = await _context.chats
                    .Where(x => x.chatRoomId == RoomId && (x.MainUserId == MainUser.Id || x.SecondUserId == MainUser.Id))
                    .Include(x => x.chatRoom)
                    .Include(x => x.MainUser)
                    .FirstOrDefaultAsync();

                    if (selectedChat == null)
                    {
                        return BadRequest("selectedChat");
                    }
                    chat.chatRooms = await _context.chats
                    .Where(x => x.MainUserId == MainUser.Id || x.SecondUserId == MainUser.Id)
                    .Select(x => x.chatRoom)
                    .ToListAsync();

                    chat.someChat = await _context.chats
                    .Where(x => x.chatRoomId == RoomId && (x.MainUserId == MainUser.Id || x.SecondUserId == MainUser.Id))
                    .Include(x => x.chatRoom)
                    .Include(x => x.MainUser)
                    .FirstOrDefaultAsync();
                chat.mainUser = await _userManager.GetUserAsync(User);

                    chat.messages = await _context.messages
                    .Where(x => x.ChatId == selectedChat.Id)
                    .Include(x => x.mainUserSender)
                    .Include(x=>x.Chat)
                    .ThenInclude(x=>x.MainUser)
                    .ToListAsync();
                }
                catch (Exception ex)
                {

                    return BadRequest("unable to create messenger"); ;
                }

            return PartialView("_PartialChatMenu", chat);
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddingNewChat(string name, string SecondUserId)
        {
            var secondUser = _userManager.Users.Where(x=>x.Id == SecondUserId).FirstOrDefault();
            UserApp mainUser = await _userManager.GetUserAsync(User);
            
            
            if(secondUser == null)
            {
                return BadRequest("Add New Chat");
            }
            try
            {
                if (_context.chatRooms.Where(x => x.ChatRoomName == name).Count() != 0 && _context.chats.Where(x => x.MainUserId == mainUser.Id || x.SecondUserId == mainUser.Id).Count() != 0)
                {
                    name += mainUser.Email;
                }
                ChatRoom room = new ChatRoom()
                {
                    ChatRoomName = name,

                };
                Chat someAddedChat = new Chat()
                {
                    chatRoom = room,
                    chatRoomId = room.ChatRoomId,
                    MainUserId = mainUser.Id,
                    MainUser = mainUser,
                    SecondUserId = secondUser.Id,
                    SecondUser = secondUser

                };

                Message message = new Message()
                {
                    MessageText = "Hi!",
                    DateOfCreatingMessage = DateTime.Now,
                    ChatId = someAddedChat.Id,
                    Chat = someAddedChat,
                    mainUserSender = mainUser,
                    MainUserId = mainUser.Id

                };

                await _context.messages.AddAsync(message);
                await _context.chats.AddAsync(someAddedChat);
                await _context.chatRooms.AddAsync(room);
                await _context.SaveChangesAsync();
                return RedirectToAction("Menu");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex);
            }
            
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddingNewMessage(string message, int chatId)
        {
            var selectedChat = await _context.chats
                .Where(x => x.Id == chatId)
                .FirstOrDefaultAsync();

            var mainUser = await _userManager.GetUserAsync(User);
            try
            {
                var newMessage = new Message()
                {
                    MessageText = message,
                    DateOfCreatingMessage = DateTime.Now,
                    ChatId = selectedChat.Id,
                    Chat = selectedChat,
                    mainUserSender = mainUser,
                    MainUserId = mainUser.Id
                };
                var roomChatId = selectedChat.chatRoomId;
                await _context.messages.AddAsync(newMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction("MenuPartial", new { RoomId = roomChatId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
