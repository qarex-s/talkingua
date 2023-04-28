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
                    var selectedChat = await _context.chats.Where(x => x.chatRoomId == RoomId && (x.MainUserId == MainUser.Id || x.SecondUserId == MainUser.Id)).Include(x => x.chatRoom).FirstOrDefaultAsync();
                    if (selectedChat == null)
                    {
                        return BadRequest("selectedChat");
                    }
                    chat.chatRooms = await _context.chats.Where(x => x.MainUserId == MainUser.Id || x.SecondUserId == MainUser.Id).Select(x => x.chatRoom).ToListAsync();
                    chat.someChat = selectedChat;
                    chat.mainUser = await _userManager.GetUserAsync(User);
                    chat.messages = await _context.messages.Where(x => x.ChatId == selectedChat.Id).Include(x => x.mainUserSender).ToListAsync();
                }
                catch(Exception ex)
                {

                    return BadRequest("unable to create messenger"); ;
                }
                
            }



            return View(chat);

        }
       
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddingNewChat(string name, string SecondUserName)
        {
            var secondUser = _userManager.Users.Where(x=>x.Name == SecondUserName).FirstOrDefault();
            UserApp mainUser = await _userManager.GetUserAsync(User);
            if(_context.chatRooms.Where(x=>x.ChatRoomName == name).Count()!=0 && _context.chats.Where(x=>x.MainUserId == mainUser.Id || x.SecondUserId == mainUser.Id).Count() != 0)
            {
                return BadRequest("You have the same called chat");
            }
            if(secondUser == null)
            {
                return BadRequest("AddingNewChat");
            }
            try
            {
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
            var selectedChat = await _context.chats.Where(x => x.Id == chatId).FirstOrDefaultAsync();
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
                return RedirectToAction("Menu", new { roomId = roomChatId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
