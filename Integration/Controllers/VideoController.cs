using Integration.BLModels;
using Integration.Mapping;
using Integration.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Integration.Controllers
{
    [Route("api/videos")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IntegrationContext _dbContext;
        

        public VideoController(IntegrationContext dbcontext)
        {
            _dbContext = dbcontext;
        }


        // GET: api/videos
        [HttpGet]
        public ActionResult<IEnumerable<BLVideo>> GetVideos()
        {

            try
            {
                var dbVideos = _dbContext.Videos
                    .Include("Genre")
                    .Include("Image")
                    .Include("VideoTags")
                    .Include("VideoTags.Tag");

                var videos = VideoMapping.MapToBL(dbVideos);

                return Ok(videos);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error fetching requested data.");
            }
        }

        // GET api/videos/5
        [HttpGet("{id}")]
        public ActionResult<BLVideo> GetVideo(int id)
        {
            try
            {
                var dbVideos = _dbContext.Videos
                    .Include("Genre")
                    .Include("Image")                   
                    .Include("VideoTags.Tag").Where(x => x.Id == id);

                if (dbVideos == null)
                {
                    return NotFound($"No results are found with given id ({id})");
                }
                var videos = VideoMapping.MapToBL(dbVideos);

                return Ok(videos);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error fetching requested data.");
            }
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<BLVideo>> Search(int page, int size, string nameFilter, string orderBy, string direction)
        {
            try
            {
            
                var searchName = HttpContext.Request.Cookies["search.name"];
                if (nameFilter != null) 
                {
                    HttpContext.Response.Cookies.Append("search.name", nameFilter);
                }
                else if (!string.IsNullOrEmpty(searchName))
                {
                    nameFilter = searchName;
                }
                //filter
                //TODO: metoda za dohvat podataka, zamijeniti var ispod
                var dbVideos = _dbContext.Videos.Where
                    (x => x.Name.Contains(searchName));

                var videos = VideoMapping.MapToBL(dbVideos);



                //order by
                if (string.Compare(orderBy, "id", true) == 0)
                {
                    videos = videos.OrderBy(x => x.Id);
                }

                else if (string.Compare(orderBy, "name", true) == 0)
                {
                    videos = videos.OrderBy(x => x.Name);
                }

                else if (string.Compare(orderBy, "totaltime", true) == 0)
                {
                    videos = videos.OrderBy(x => x.TotalSeconds);
                }
                else
                {
                    //default: id
                    videos = videos.OrderBy(x => x.Id);
                }
                if (string.Compare(direction, "desc", true) == 0)
                {
                    videos = videos.Reverse();
                }


                //paging

                //page i size predstavljaju broj stranice i broj elemenata po stranici
                videos = videos.Skip(page * size).Take(size);


                //session handling
                HttpContext.Session.SetString("videos.search.count", videos.Count().ToString());

                return Ok(videos);
                
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error while searching for data");
            }
        }

        [HttpGet("[action]")]
        public ActionResult<int> GetLastVideosCount()
        {
            var count = HttpContext.Session.GetString("videos.search.count");
            return Ok(count);
        }




        // POST api/videos
        [HttpPost()]
        public ActionResult<BLVideo> Post(BLVideo video)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var dbVideo = VideoMapping.MapToDAL(video);

                var dbTags = _dbContext.Tags.Where(x => video.Tags.Contains(x));

                dbVideo.VideoTags = dbTags.Select(x => new VideoTag 
                    {Tag = x}).ToList();

                _dbContext.Videos.Add(dbVideo);

                _dbContext.SaveChanges();

                video = VideoMapping.MapToBL(dbVideo);

                return Ok(video);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error inserting data.");
            }
        }

        // PUT api/videos/5
        [HttpPut("{id}")]
        public ActionResult<BLVideo> Put(int id, BLVideo video)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var dbVideo = _dbContext.Videos.FirstOrDefault
                    (x => x.Id == id);

                if (dbVideo == null)
                {
                    return NotFound($"No results are found with given id ({id})");
                }

                dbVideo.Name= video.Name;
                dbVideo.GenreId = video.GenreId;
                dbVideo.TotalSeconds = video.TotalSeconds;
                dbVideo.Description = video.Description;
                dbVideo.ImageId = video.ImageId;
                dbVideo.StreamingUrl = video.StreamingUrl;

                //brisi stare tagove
                var tagToRemove = dbVideo.VideoTags.Where
                    (x => !video.Tags.Contains(x.Tag));
                foreach (var rmvTag in tagToRemove)
                {
                    _dbContext.VideoTags.Remove(rmvTag);
                }

                //dodaj nove tagove
                var existingDbTags = dbVideo.VideoTags.Select(x => x.Tag);
                var newTags = video.Tags.Except(existingDbTags);
                foreach (var newTag in newTags)
                {
                    var dbTag = _dbContext.Tags.FirstOrDefault(x => newTag == x);
                    //nema tagova
                    if (dbTag == null)
                        continue;

                    dbVideo.VideoTags.Add(new VideoTag
                    {
                        Video = dbVideo,
                        Tag = dbTag
                    });
                }

                _dbContext.SaveChanges();

                return Ok(dbVideo);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error inserting data.");
            }
        }

        // DELETE api/videos/id
        [HttpDelete("{id}")]
        public ActionResult<BLVideo> Delete(int id)
        {
            try
            {
                var dbAudio = _dbContext.Videos.FirstOrDefault
            (x => x.Id == id);
                if (dbAudio == null)
                {
                    return NotFound($"No results are found with given id ({id})");
                }
                _dbContext.Videos.Remove(dbAudio);

                _dbContext.SaveChanges();

                return Ok(dbAudio);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error while deleting the data");
            }
        }
    }
}
