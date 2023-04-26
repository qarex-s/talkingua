using TalkingUADev.Areas.Identity.Data;
using TalkingUADev.Models;

namespace TalkingUADev.Util
{
    public class UtilAllPostProp
    {
        public List<UserPost> userPosts { get; set; }
        public List<UserComment> userComments { get; set; }
        public UserApp user { get; set; }
    }
}
