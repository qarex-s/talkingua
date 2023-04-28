using System.ComponentModel.DataAnnotations.Schema;
using TalkingUADev.Areas.Identity.Data;

namespace TalkingUADev.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string MessageText { get; set; }
        [ForeignKey("Chat")]
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        [ForeignKey("mainUserSender")]
        public string MainUserId { get; set; }
        public UserApp mainUserSender { get; set; }
        public DateTime DateOfCreatingMessage { get; set; }
    }
}
