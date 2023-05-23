using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TalkingUADev.Areas.Identity.Data;

namespace TalkingUADev.Models
{
    public class UserPost
    {
        public Guid UserPostId { get; set; }
        public string? Image { get; set; } = null;
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Tags { get; set; }
        public string Position { get; set; }
        public int Likes { get; set; } = 0;
        [ForeignKey("user")]
        public string UserAppId { get; set; }
        public UserApp user { get; set; }
        public DateTime DateOfCreatingPost { get; set; }= DateTime.Now;
        public List<LikeUser> likes = new List<LikeUser>();
        public List<UserComment> comments = new List<UserComment>();
        public string? ImageTitle { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }



    }
}
