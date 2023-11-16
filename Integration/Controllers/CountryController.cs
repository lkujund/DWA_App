using Integration.BLModels;
using Integration.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Integration.Controllers
{
    [Route("api/countries")]
    [ApiController]
    public class CountryController : ControllerBase
    {
            private readonly IntegrationContext _dbContext;


            public CountryController(IntegrationContext dbcontext)
            {
                _dbContext = dbcontext;
            }
        [HttpGet("get")]
        public ActionResult<IEnumerable<Country>> GetCountries()
        {

            try
            {
                var dbCountries = _dbContext.Countries;

                return Ok(dbCountries);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error fetching requested data.");
            }
        }

        [HttpPost]
        public ActionResult<Country> Post(BLCountry country)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Country target = new Country {                   
                    Name= country.Name,
                    Code= country.Code
                };
                _dbContext.Countries.Add(target);

                _dbContext.SaveChanges();

                return Ok(country);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error inserting data.");
            }
        }


    }
}
