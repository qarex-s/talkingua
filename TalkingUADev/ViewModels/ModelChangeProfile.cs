using System.ComponentModel.DataAnnotations;

namespace TalkingUADev.ViewModels
{
    public class ModelChangeProfile
    {
        [Required]
        public Guid UserPostId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Desc { get; set; }
        [Required]
        public string Position { get; set; }    
    }
}
