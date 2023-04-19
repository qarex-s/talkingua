namespace TalkingUADev.Models
{
    public class LikeUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PostId { get; set; }
        public bool isLiked { get; set; }
    }
}
