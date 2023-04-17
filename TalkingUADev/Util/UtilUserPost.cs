using TalkingUADev.Areas.Identity.Data;
using TalkingUADev.Models;

namespace TalkingUADev.Util
{
    public class UtilUserPost
    {
        private UserApp _userApp;
        private List<UserPost> _userPosts;

        public void SetUserAppUtil(UserApp userApp)
        {
            _userApp = userApp;
        }
        public void SetUserPostsUtil(List<UserPost> userPosts)
        {
            _userPosts = userPosts;
        }
        public UserApp GetUserAppUtil()
        {
            return _userApp;
        }
        public List<UserPost> GetUserPostUtil()
        {
            return _userPosts;
        }
    }
}
