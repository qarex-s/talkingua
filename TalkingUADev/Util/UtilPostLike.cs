using TalkingUADev.Areas.Identity.Data;
using TalkingUADev.Models;

namespace TalkingUADev.Util
{
    public class UtilPostLike
    {
       public UserPost userPost { get; set; }
       public List<LikeUser> userLike { get; set; } 
       public List<UserComment> postComments { get; set; } 
       public List<UserApp> usersComments { get; set; }
       

    }
}
