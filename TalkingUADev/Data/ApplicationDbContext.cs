using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TalkingUADev.Areas.Identity.Data;

namespace TalkingUADev.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserApp>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}