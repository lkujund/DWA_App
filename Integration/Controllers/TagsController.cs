using Azure;
using Integration.BLModels;
using Integration.Mapping;
using Integration.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Integration.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IntegrationContext _dbContext;

        public TagsController(IntegrationContext dbcontext)
        {
            _dbContext = dbcontext;
        }



        [HttpGet("[action]")]
        public ActionResult<IEnumerable<BLTag>> GetAll()
        {
            try
            {
                var dbTags = _dbContext.Tags.Include("VideoTags.Video");

                if (dbTags == null)
                {
                    return NotFound($"Could not find any tags");
                }

                return Ok(TagMapping.MapToBL(dbTags));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error fetching the data");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<BLTag> Get(int id)
        {
             try
            {
                var dbTag = _dbContext.Tags.Include("VideoTags.Video").FirstOrDefault
                    (x => x.Id == id);
                if (dbTag == null)
                {
                    return NotFound($"Could not find any tag with given id ({id})");
                }


                return Ok(TagMapping.MapToBL(dbTag));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error fetching the data");
            }
        }

        [HttpPost]
        public ActionResult<BLTag> Post(BLTag tag)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var dbTag = TagMapping.MapToDAL(tag);

                _dbContext.Tags.Add(dbTag);

                _dbContext.SaveChanges();

                tag = TagMapping.MapToBL(dbTag);

                return Ok(tag);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error while inserting the data");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<BLTag> Put(int id, BLTag tag)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var dbTag = _dbContext.Tags.FirstOrDefault
                    (x => x.Id == id);

                if (dbTag == null)
                {
                    return NotFound($"Could not find any tag with given id ({id})");
                }

                dbTag.Name = tag.Name;

                _dbContext.SaveChanges();

                return Ok(TagMapping.MapToBL(dbTag));
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error while inserting the data");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<BLTag> Delete(int id)
        {
            try
            {
                var dbTag = _dbContext.Tags.FirstOrDefault
            (x => x.Id == id);


                if (dbTag == null)
                {
                    return NotFound($"Could not find any tag with given id ({id})");
                }

                _dbContext.Tags.Remove(dbTag);

                _dbContext.SaveChanges();

                return Ok(TagMapping.MapToBL(dbTag));
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error while deleting the data");
            }
        }
    }
}
