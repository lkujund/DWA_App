using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Integration.Repositories;
using Application.Services;
using Application.Viewmodels;

namespace Application.Controllers
{
    public class AppController : Controller
    {
        private readonly IAppRepository _appRepo;
        private readonly IMailService _mailService;
        public AppController(IAppRepository appRepo, IMailService mailService)
        {
            _appRepo = appRepo;
            _mailService = mailService;
        }
        public IActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Home");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(VMLogin login)
        {
            if (!ModelState.IsValid)
                return View(login);

            var user = _appRepo.GetConfirmedUser(
                login.Username,
                login.Password);

            if (user == null)
            {
                ModelState.AddModelError("Username", "Invalid username or password");
                return View(login);
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Email) };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var properties = new AuthenticationProperties
            {
                IsPersistent = login.StaySignedIn,
            };
            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties).Wait();

            if (login.RedirectUrl != null)
            {
                return Redirect(login.RedirectUrl);
            }
            else
            {
                return RedirectToAction("Home");
            }
        }
        public IActionResult UserRegistered()
        {
            return View();
        }
        public IActionResult ChangePass(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var blUser = _appRepo.GetUser(id);
                if (blUser == null)
                {
                    ModelState.AddModelError("Id", "User with specified ID doesn't exist");
                    return RedirectToAction("Home");
                }
                var changePass = new VMChangePass
                {
                    Username = blUser.Username
                };
                return View(changePass);
            }
            return View("Login");
        }
        [HttpPost]
        public IActionResult ChangePass(VMChangePass changePassword)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                _appRepo.ChangePassword(changePassword.Username, changePassword.NewPassword);

                return RedirectToAction("Home");
            }
            return View("Login");

        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();

            return View("Login");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(VMRegister register)
        {
            if (!ModelState.IsValid)
                return View(register);

            if (_appRepo.CheckUsernameExists(register.Username))
            {
                ModelState.AddModelError("Username", "Username already exists");
                return View(register);
            }

            if (_appRepo.CheckEmailExists(register.Email))
            {
                ModelState.AddModelError("Email", "E-mail already exists");
                return View(register);
            }

            var user = _appRepo.CreateUser(
                register.Username,
                register.FirstName,
                register.LastName,
                register.Email,
                register.Password,
                register.CountryId);

            _mailService.Send(user.Email, user.SecurityToken);

            return RedirectToAction("UserRegistered");
        }
        public IActionResult Home()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var videos = _appRepo.GetAll();
                return View(videos);
            }
            return View("Login");
        }
        public IActionResult Profile()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string? name = HttpContext.User.Identity.Name;
                var bLUser = _appRepo.ViewProfile(name);
                return View(bLUser);
            }
            return View("Login");
        }
        public IActionResult Video(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var videos = _appRepo.GetAll();
                var video = videos.FirstOrDefault(x => x.Id == id);
                return View(video);
            }
            return View("Login");
        }

        public IActionResult ValidateEmail(VMValidateMail validateEmail)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Login");

            _appRepo.ConfirmEmail(
                validateEmail.Email,
                validateEmail.SecurityToken);

            return RedirectToAction("Login");
        }
    }
}
