using TalkingUADev.Areas.Identity.Data;
using TalkingUADev.Models;

namespace TalkingUADev.Util
{
    public class UtilChat
    {
        public List<ChatRoom> chatRooms { get; set; }
        public List<Chat> ListChats { get; set; } = null;
        public Chat someChat { get; set; }
        public ChatRoom choosedRoom { get; set; }   
        public List<Message> messages { get; set; }   
        public UserApp mainUser { get; set; }
        public List<UserApp> FriendsUser { get; set; }
        public UserApp UserInvoker { get; set; }
        public UserApp SecondUserIvoker { get; set; }
    }
}
