using Integration.BLModels;
using Integration.Mapping;
using Integration.Models;
using Integration.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Administration.Controllers
{
    public class TagAdminController : Controller
    {
        private readonly IntegrationContext _dbContext;
        private readonly ITagRepository _tagRepo;

        public TagAdminController(IntegrationContext dbContext, ITagRepository tagRepo)
        {
            _dbContext = dbContext;
            _tagRepo = tagRepo;
        }

        public IActionResult Tags()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var dbTags = _dbContext.Tags;
                var blTags = TagMapping.MapToBL(dbTags);
                return View(blTags);
            }
            return View("Index");
        }

        public IActionResult Add()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            return View("Index");
        }
        [HttpPost]
        public IActionResult Add(BLTag tag)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                _tagRepo.AddTag(tag);
                return RedirectToAction("Tags");
            }
            return View("Index");
        }


        public IActionResult Tag(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var target = _dbContext.Tags.FirstOrDefault(x => x.Id == id);
                if(target == null)
                {
                    ModelState.AddModelError("Id", "No such tag found!");
                    return RedirectToAction("Tags");
                }

                var blTag = TagMapping.MapToBL(target);

                return View(blTag);
            }
            return View("Index");
        }



        public IActionResult Edit(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var target = _dbContext.Tags.FirstOrDefault(x => x.Id == id);
                if (target == null)
                {
                    ModelState.AddModelError("Id", "No such tag found!");
                    return RedirectToAction("Tags");
                }

                var blTag = TagMapping.MapToBL(target);

                return View(blTag);
            }
            return View("Index");
        }
        [HttpPost]
        public IActionResult Edit(BLTag tag)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                _tagRepo.EditTag(tag);

                return RedirectToAction("Tags");
            }
            return View("Index");
        }




        public IActionResult Delete(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var target = _dbContext.Tags.FirstOrDefault(x => x.Id == id);
                if (target == null)
                {
                    ModelState.AddModelError("Id", "No such tag found!");
                    return RedirectToAction("Tags");
                }

                var blTag = TagMapping.MapToBL(target);

                return View(blTag);
            }
            return View("Index");
        }
        [HttpPost]
        public IActionResult Delete(BLTag tag)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                _tagRepo.DeleteTag(tag);
                return RedirectToAction("Tags");
            }
            return View("Index");
        }
    }
}
