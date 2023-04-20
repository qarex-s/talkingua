using System.ComponentModel.DataAnnotations.Schema;
using TalkingUADev.Areas.Identity.Data;

namespace TalkingUADev.Models
{
    public class UserComment
    {
        public int Id { get; set; }
        public string TextMessage { get; set; }
        [ForeignKey("userApp")]
        public string FromUserId { get; set; }
        public UserApp userApp { get; set; }
        [ForeignKey("post")]
        public Guid ToPostId { get; set; }
        public UserPost post { get; set; }
        public DateTime DateOfCreatingComment { get; set; } = DateTime.Now;

    }
}
