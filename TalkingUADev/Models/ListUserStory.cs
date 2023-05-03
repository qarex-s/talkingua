using System.ComponentModel.DataAnnotations.Schema;
using TalkingUADev.Areas.Identity.Data;

namespace TalkingUADev.Models
{
    public class ListUserStory
    {
        public int Id { get; set; }
        [ForeignKey("UserOfStore")]
        public string UserId { get; set; }
        public UserApp UserOfStore { get; set; }
        public List<UserStory> Stories = new List<UserStory>();
        public bool isActiveListStories { get; set; } = true;
    }
}
