using Integration.BLModels;
using Integration.Mapping;
using Integration.Models;

namespace Integration.Repositories
{
    public interface IVideoRepository
    {
        IEnumerable<BLVideo> GetAll();
        BLVideo AddVideo(string name, string description, int genreId, int imageId, int totalSeconds, string url, List<Tag> tags);
        BLVideo GetVideo(int id);
        BLVideo EditVideo(BLVideo video);
        void DeleteVideo(int id);
    }
    public class VideoRepository : IVideoRepository
    {
        private readonly IntegrationContext _dbContext;
        public VideoRepository(IntegrationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public BLVideo AddVideo(string name, string description, int genreId, int imageId, int totalSeconds, string url, List<Tag> tags)
        {
            var dbVideo = new Video
            {
                Name = name,
                Description = description,
                CreatedAt = DateTime.UtcNow,
                GenreId = genreId,
                ImageId = imageId,
                TotalSeconds = totalSeconds,
                StreamingUrl = url
            };

            _dbContext.Videos.Add(dbVideo);

            _dbContext.SaveChanges();

            var blVideo = VideoMapping.MapToBL(dbVideo);

            return blVideo;
        }

        public void DeleteVideo(int id)
        {
            var target = _dbContext.Videos.FirstOrDefault(x => x.Id == id);
            if (target != null)
            {
                _dbContext.Remove(target);
                _dbContext.SaveChanges();
            }
        }

        public BLVideo EditVideo(BLVideo video)
        {
            var target = _dbContext.Videos.FirstOrDefault(x => x.Id == video.Id);

            target.Name= video.Name;
            target.Description= video.Description;
            target.GenreId= video.GenreId;
            target.ImageId= video.ImageId;
            target.TotalSeconds= video.TotalSeconds;
            target.StreamingUrl= video.StreamingUrl;

            var blVideo = VideoMapping.MapToBL(target);

            _dbContext.SaveChanges();

            return blVideo;
        }

        public IEnumerable<BLVideo> GetAll()
        {
            var dbVideos = _dbContext.Videos;

            var blVideos = VideoMapping.MapToBL(dbVideos);

            return blVideos;
        }

        public BLVideo GetVideo(int id)
        {
            var dbVideo = _dbContext.Videos.FirstOrDefault(x => x.Id == id);
            var blVideo = VideoMapping.MapToBL(dbVideo);

            return blVideo;
        }
    }
}
