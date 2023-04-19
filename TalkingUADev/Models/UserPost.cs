using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalkingUADev.Models
{
    public class UserPost
    {
        public Guid UserPostId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Tags { get; set; }
        public string Position { get; set; }
        public int Likes { get; set; } = 0;
        public string UserAppId { get; set; }
        public DateTime DateOfCreatingPost { get; set; }= DateTime.Now;
        public List<LikeUser> likes = new List<LikeUser>();


    }
}
