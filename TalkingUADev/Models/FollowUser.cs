namespace TalkingUADev.Models
{
    public class FollowUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FollowerId { get; set; }
        public bool isFollowed { get; set; } = false;
        public DateTime DateOfFollowing { get; set; }
    }
}
