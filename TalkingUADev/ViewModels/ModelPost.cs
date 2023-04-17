using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = Microsoft.Build.Framework.RequiredAttribute;

namespace TalkingUADev.ViewModels
{
    public class ModelPost
    {
        [Required]
        [Display(Name = "Image")]
        public string Image { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        public string Desc { get; set; } = null;
        public string Tags { get; set; } = null;
        [Required]
        [Display(Name = "Position")]
        public string Position { get; set; }
        public int Likes { get; set; } = 0;
        public string UserAppId { get; set; }
    }
}
