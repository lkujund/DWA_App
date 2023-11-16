using Integration.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Administration.Controllers
{
    public class CountryAdminController : Controller
    {
        private readonly ICountryRepository _countryRepo;

        public CountryAdminController(ICountryRepository countryRepo)
        {
            _countryRepo = countryRepo;
        }

        public IActionResult Countries()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var countries = _countryRepo.GetAll();
                return View(countries);
            }
            return View("Index");
        }
    }
}
