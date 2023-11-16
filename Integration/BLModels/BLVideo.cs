using Integration.Models;
using System.ComponentModel.DataAnnotations;

namespace Integration.BLModels
{
    public class BLVideo
    {
        public int? Id { get; set; }
        public string Name{ get; set; }
        public string Description{ get; set; }
        public int GenreId { get; set; }
        public int? ImageId { get; set; }
        public int TotalSeconds { get; set; }
        [Url]
        public string StreamingUrl { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
