using Integration.BLModels;
using Integration.Mapping;
using Integration.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Integration.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IntegrationContext _dbContext;

        public GenreController(IntegrationContext dbcontext)
        {
            _dbContext = dbcontext;
        }


        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Genre>> GetAll()
        {
            try
            {
                var genres = _dbContext.Genres.Include("Videos");
                return Ok(genres);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error fetching the data");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<BLGenre> Get(int id)
        {
            try
            {
                var dbGenre = _dbContext.Genres.FirstOrDefault
                    (x => x.Id == id);
                if (dbGenre == null)
                {
                    return NotFound($"Could not find any genre with given id ({id})");
                }

                return Ok(dbGenre);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error fetching the data");
            }
        }


        [HttpPost()]
        public ActionResult<BLGenre> Post(BLGenre genre)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var dbGenre = GenreMapping.MapToDAL(genre);

                _dbContext.Genres.Add(dbGenre);

                _dbContext.SaveChanges();

                genre = GenreMapping.MapToBL(dbGenre);

                return Ok(genre);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error while inserting the data");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<BLGenre> Put(int id, BLGenre genre)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var dbGenre = _dbContext.Genres.FirstOrDefault
                    (x => x.Id == id);

                if (dbGenre == null)
                {
                    return NotFound($"Could not find any genre with given id ({id})");
                }

                

                dbGenre.Name = genre.Name;
                dbGenre.Description = genre.Description;

                _dbContext.SaveChanges();

                return Ok(dbGenre);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error while inserting the data");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<BLGenre> Delete(int id)
        {
            try
            {
                var dbGenre = _dbContext.Genres.FirstOrDefault
            (x => x.Id == id);


                if (dbGenre == null)
                {
                    return NotFound($"Could not find any genre with given id ({id})");
                }
                var blGenre = GenreMapping.MapToBL(dbGenre);

                _dbContext.Genres.Remove(dbGenre);

                _dbContext.SaveChanges();

                return Ok(blGenre);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error while deleting the data");
            }
        }
    }
}
