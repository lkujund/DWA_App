using Integration.BLModels;
using Integration.Controllers;
using Integration.Mapping;
using Integration.Models;
using Integration.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Administration.Controllers
{
    public class VideoAdminController : Controller
    {
        private readonly IVideoRepository _videoRepo;
        public VideoAdminController(IVideoRepository videoRepo)
        {
            _videoRepo = videoRepo;
        }

        public ActionResult Videos()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var blVideos = _videoRepo.GetAll();

                return View(blVideos);
            }
            return View("Index");
        }

        public ActionResult Edit(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var blVideo = _videoRepo.GetVideo(id);
                if (blVideo == null)
                {
                    ModelState.AddModelError("Id", "No such video found!");
                    return RedirectToAction("Videos");
                }
                return View(blVideo);
            }
            return View("Index");
        }
        [HttpPost]

        public ActionResult Edit(BLVideo video)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                _videoRepo.EditVideo(video);
                return RedirectToAction("Videos");
            }
            return View("Index");
        }
        public ActionResult Add()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult Add(BLVideo video)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                _videoRepo.AddVideo(
                    video.Name,
                    video.Description,
                    video.GenreId,
                    (int)video.ImageId,
                    video.TotalSeconds,
                    video.StreamingUrl,
                    video.Tags
                    );
                return RedirectToAction("Videos");
            }
            return View("Index");
        }
        public ActionResult Delete(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var blVideo = _videoRepo.GetVideo(id);
                if (blVideo == null) {
                    ModelState.AddModelError("Id", "No such video found!");
                    return RedirectToAction("Videos");
                }
                return View(blVideo);
            }
            return View("Index");
        }
        [HttpPost]
        public ActionResult Delete(BLVideo video)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                _videoRepo.DeleteVideo((int)video.Id);

                return RedirectToAction("Videos");
            }
            return View("Index");
        }
        public ActionResult Video(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var blVideo = _videoRepo.GetVideo(id);
                if (blVideo == null)
                {
                    ModelState.AddModelError("Id", "No such video found!");
                    return RedirectToAction("Videos");
                }
                return View(blVideo);
            }
            return View("Index");
        }

    }
}
