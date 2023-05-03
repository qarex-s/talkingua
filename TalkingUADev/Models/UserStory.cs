using System.ComponentModel.DataAnnotations.Schema;

namespace TalkingUADev.Models
{
    public class UserStory
    {
        public int Id { get; set; }
        public string ImageStory { get; set; }
        public int CountWathcedStory { get; set; } = 0;
        public DateTime DateOfCreatingStory { get; set; } = DateTime.Now;
        [ForeignKey("listUserStory")]
        public int ListUserStoryId { get; set; }
        public ListUserStory listUserStory { get; set; }
        public bool isActiveStory { get; set; } = true;
        public List<WatchedStoriesByUser> watchedStories = new List<WatchedStoriesByUser>();
    }
}
