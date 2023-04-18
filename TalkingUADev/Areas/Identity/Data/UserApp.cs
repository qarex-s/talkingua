using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TalkingUADev.Models;

namespace TalkingUADev.Areas.Identity.Data;

// Add profile data for application users by adding properties to the UserApp class
public class UserApp : IdentityUser
{
    public string Image { get; set; }
    public string Name{ get; set; }
    public string FullName { get; set; }
    public string City { get; set; }
    public DateTime DateBirthd { get; set; }
    public DateTime DateOfCreateUser { get; set; }

    public int CountSubs { get; set; } = 0;
    public int CountFollows { get; set; } = 0;
    public int CountPosts { get; set; } = 0;

    public List<UserPost> posts = new List<UserPost>();
    
    public List<FollowUser> followUsers = new List<FollowUser>();

    
}

