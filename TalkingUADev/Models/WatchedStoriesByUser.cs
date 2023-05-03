using System.ComponentModel.DataAnnotations.Schema;
using TalkingUADev.Areas.Identity.Data;

namespace TalkingUADev.Models
{
    public class WatchedStoriesByUser
    {
        public int Id { get; set; }
        [ForeignKey("mainUser")]
        public string UserId { get; set; }
        public UserApp mainUser { get; set; }
        [ForeignKey("someStory")]
        public int StoriesId { get; set; }
        public UserStory someStory { get; set; }
    }
}
