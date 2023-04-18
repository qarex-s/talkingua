using TalkingUADev.Areas.Identity.Data;
using TalkingUADev.Models;

namespace TalkingUADev.Util
{
    public class UtilUserPost
    {
        private UserApp _userApp;
        private List<UserPost> _userPosts;
        private bool UserIsFollowed;

        public void SetUserAppUtil(UserApp userApp)
        {
            _userApp = userApp;
        }
        public void SetUserPostsUtil(List<UserPost> userPosts)
        {
            _userPosts = userPosts;
        }
        public void SetUserIsFollowed(bool _value = false)
        {
            UserIsFollowed = _value;
        }

        public void SetAll(UserApp userApp,List<UserPost> userPosts)
        {
            _userApp = userApp;
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
        public bool GetUserIsFollowed()
        {
            return UserIsFollowed;
        }
    }
}
