using Integration.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Integration.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private static IList<Notification> _notifications = new List<Notification>();



        [HttpGet("[action]")]
        public ActionResult<Notification> GetAll()
        {
            return Ok(_notifications);
        }

        [HttpGet("{id}")]
        public ActionResult<Notification> Get(int id)
        {
            var target = _notifications.FirstOrDefault(x => x.Id == id);
            return Ok(target);
        }

        [HttpPost]
        public ActionResult<Notification> Post([FromBody] Notification notification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_notifications.Any())
            {
                var nextId = _notifications.Max(x => x.Id) + 1;
                notification.Id = nextId;
            }
            else
            {
                notification.Id = 1;
            }
            _notifications.Add(notification);

            return Ok(notification);
        }

        [HttpPost("[action]")]
        public ActionResult Form([FromForm] FormData data)
        {
            return Redirect("/notification-form.html");
        }


        [HttpPut("{id}")]
        public ActionResult<Notification> Put(int id, [FromBody] Notification notification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var target = _notifications.FirstOrDefault(x => x.Id == id);
            if (target != null)
            {
                target.Id = id;
                target.ReceiverEmail = notification.ReceiverEmail;
                target.Subject = notification.Subject;
                target.Body = notification.Body;
            }
            else
            {
                return NotFound();
            }
            return Ok(target);
        }

        [HttpDelete("{id}")]
        public ActionResult<Notification> Delete(int id)
        {
            var target = _notifications.FirstOrDefault(x => x.Id == id);
            if (target == null)
            {
                return NotFound();
            }
            _notifications.Remove(target);

            return Ok(target);
        }
    }
}
