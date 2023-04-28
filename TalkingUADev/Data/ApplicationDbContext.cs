using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TalkingUADev.Areas.Identity.Data;
using TalkingUADev.Models;

namespace TalkingUADev.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserApp>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
       public DbSet<UserPost> Posts { get; set; }
       public DbSet<FollowUser> followUsers { get; set; }
       public DbSet<LikeUser> likesUsers{ get; set; }
       public DbSet<UserComment> commentsUsers{ get; set; }
       
        public DbSet<Message> messages { get; set; }
        public DbSet<Chat> chats { get; set; }
        public DbSet<ChatRoom> chatRooms { get; set; }
    }
}