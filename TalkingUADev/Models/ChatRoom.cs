using System.ComponentModel.DataAnnotations.Schema;

namespace TalkingUADev.Models
{
    public class ChatRoom
    {
        public int ChatRoomId { get; set; }
        public string ChatRoomName { get; set; }
        public List<Chat> chats { get; set; }
    }
    
}
