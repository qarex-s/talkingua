using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xunit;
using RequiredAttribute = Microsoft.Build.Framework.RequiredAttribute;

namespace TalkingUADev.ViewModels
{
    public class ModelPost
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        public string Desc { get; set; } = null;
        [Required]
        public string Tags { get; set; }
        [Required]
        [Display(Name = "Position")]
        public string Position { get; set; }
        public int Likes { get; set; } = 0;
        public string? ImageTitle { get; set; }
        [Required]
        [Display(Name = "ImageFile")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
