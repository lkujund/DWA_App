using Integration.BLModels;
using Integration.Mapping;
using Integration.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Integration.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IntegrationContext _dbContext;


        public UserController(IntegrationContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BLUser>> GetUsers()
        {

            try
            {
                var dbUsers = _dbContext.Users
                    .Include("Country");

                var users = UserMapping.MapToBL(dbUsers);

                return Ok(users);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error fetching requested data.");
            }
        }

        [HttpPost()]
        public ActionResult<BLUser> Post(BLUser user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var dbUser = UserMapping.MapToDAL(user);

                dbUser.IsConfirmed = true;

                _dbContext.Users.Add(dbUser);

                _dbContext.SaveChanges();


                return Ok(user);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error inserting data.");
            }
        }
    }
}
