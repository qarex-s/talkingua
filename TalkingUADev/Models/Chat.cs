using System.ComponentModel.DataAnnotations.Schema;
using TalkingUADev.Areas.Identity.Data;

namespace TalkingUADev.Models
{
    public class Chat
    {
        public int Id { get; set; }
        [ForeignKey("MainUser")]
        public string MainUserId { get; set; }
        public UserApp MainUser { get; set; }
        [ForeignKey("SecondUser")]
        public string SecondUserId { get; set; }
        public UserApp SecondUser { get; set; }
        [ForeignKey("chatRoom")]
        public int chatRoomId { get; set; }
        public ChatRoom chatRoom { get; set; }
        public List<Message> someMessages = new List<Message>();

    }

}
