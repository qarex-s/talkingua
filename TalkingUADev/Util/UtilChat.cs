using TalkingUADev.Areas.Identity.Data;
using TalkingUADev.Models;

namespace TalkingUADev.Util
{
    public class UtilChat
    {
        public List<ChatRoom> chatRooms { get; set; }
        public Chat someChat { get; set; }
        public List<Message> messages { get; set; }   
        public UserApp mainUser { get; set; }
    }
}
