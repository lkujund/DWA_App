using Integration.BLModels;
using Integration.Mapping;
using Integration.Models;
using Integration.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Administration.Controllers
{
    public class GenreAdminController : Controller
    {
        private readonly IntegrationContext _dbContext;
        private readonly IGenreRepository _genreRepo;

        public GenreAdminController(IntegrationContext dbContext, IGenreRepository genreRepo)
        {
            _dbContext = dbContext;
            _genreRepo = genreRepo;
        }
        public IActionResult Genres()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var dbGenres = _dbContext.Genres;
                var blGenres = GenreMapping.MapToBL(dbGenres);
                return View(blGenres);
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
        public IActionResult Add(BLGenre genre)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                _genreRepo.AddGenre(genre);
                return RedirectToAction("Genres");
            }
            return View("Index");
        }


        public IActionResult Genre(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var target = _dbContext.Genres.FirstOrDefault(x => x.Id == id);
                if (target == null)
                {
                    ModelState.AddModelError("Id", "No such genre found!");
                    return RedirectToAction("Genres");
                }

                var blGenres = GenreMapping.MapToBL(target);

                return View(blGenres);
            }
            return View("Index");
        }


        public IActionResult Edit(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var target = _dbContext.Genres.FirstOrDefault(x => x.Id == id);
                if (target == null)
                {
                    ModelState.AddModelError("Id", "No such genre found!");
                    return RedirectToAction("Genres");
                }

                var blGenres = GenreMapping.MapToBL(target);

                return View(blGenres);
            }
            return View("Index");
        }
        [HttpPost]
        public IActionResult Edit(BLGenre genre)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                _genreRepo.EditGenre(genre);

                return RedirectToAction("Genres");
            }
            return View("Index");
        }


        public IActionResult Delete(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var target = _dbContext.Genres.FirstOrDefault(x => x.Id == id);
                if (target == null)
                {
                    ModelState.AddModelError("Id", "No such genre found!");
                    return RedirectToAction("Genres");
                }

                var blGenres = GenreMapping.MapToBL(target);

                return View(blGenres);
            }
            return View("Index");
        }
        [HttpPost]
        public IActionResult Delete(BLGenre genre)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                _genreRepo.DeleteGenre(genre);
                return RedirectToAction("Genres");
            }
            return View("Index");
        }
    }
}
