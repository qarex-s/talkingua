using System.ComponentModel.DataAnnotations;

namespace TalkingUADev.ViewModels
{
    public class ModelStory
    {
        [Required]
        [Display(Name = "ImageFile")]
        public IFormFile ImageFile { get; set; }
    }
}
