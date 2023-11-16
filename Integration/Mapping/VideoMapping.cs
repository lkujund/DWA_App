using Integration.BLModels;
using Integration.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Integration.Mapping
{
    public class VideoMapping
    {
        public static IEnumerable<BLVideo> MapToBL(IEnumerable<Video> videos) =>
     videos.Select(x => MapToBL(x));

        public static BLVideo MapToBL(Video video) =>
            new BLVideo
            {
                Id = video.Id,
                Name = video.Name,
                Description = video.Description,
                GenreId = video.GenreId,
                ImageId = video.ImageId,
                TotalSeconds = video.TotalSeconds,
                StreamingUrl = video.StreamingUrl,
                Tags = video.VideoTags.Select(x => x.Tag).ToList()
            };

        public static IEnumerable<Video> MapToDAL(IEnumerable<BLVideo> blVideos) 
            => blVideos.Select(x => MapToDAL(x));

        public static Video MapToDAL(BLVideo video) =>
            new Video
            {
                Id = video.Id ?? 0,
                Name = video.Name,
                Description = video.Description,
                CreatedAt = DateTime.Now,
                GenreId = video.GenreId,
                ImageId= video.ImageId,
                TotalSeconds = video.TotalSeconds,
                StreamingUrl = video.StreamingUrl
            };
    }
}
