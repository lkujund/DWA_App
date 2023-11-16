using Integration.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Administration.Viewmodels
{
    public class VMVideo
    {

        public int Id { get; set; }


        [DisplayName("Video name")]
        [Required]
        public string Name { get; set; } = null!;

        [DisplayName("Description")]
        [Required]
        public string? Description { get; set; }

        [DisplayName("Duration (seconds)")]
        [Required]
        public int TotalSeconds { get; set; }

        [DisplayName("Video URL")]
        [Required]
        public string? StreamingUrl { get; set; }

        [DisplayName("Genre")]
        [Required]
        public virtual Genre Genre { get; set; } = null!;

        [DisplayName("Image")]
        [Required]
        public virtual Image? Image { get; set; }

        [DisplayName("Tags")]
        public virtual ICollection<VideoTag> VideoTags { get; set; } = new List<VideoTag>();
        

    }
}
