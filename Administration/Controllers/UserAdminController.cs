using Administration.Viewmodels;
using Integration.Models;
using Integration.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Administration.Services;

namespace Administration.Controllers
{
    public class UserAdminController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IMailService _mailService;
        public UserAdminController(IUserRepository userRepo, IMailService mailService)
        {
            _userRepo = userRepo;
            _mailService = mailService;
        }
        public IActionResult Users()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var blUsers = _userRepo.GetAll();

                return View(blUsers);
            }
            return View("Login");
        }
        public IActionResult User(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var blUser = _userRepo.GetUser(id);
                return View(blUser);
            }
            return View("Login");
        }

        public IActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Users");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(VMLogin login)
        {
            if (!ModelState.IsValid)
                return View(login);

            var user = _userRepo.GetConfirmedUser(
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
                return RedirectToAction("Users");
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();

            return View("Login");
        }


        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(VMRegister register)
        {
            if (!ModelState.IsValid)
                return View(register);

            if (_userRepo.CheckUsernameExists(register.Username))
            {
                ModelState.AddModelError("Username", "Username already exists");
                return View(register);
            }

            if (_userRepo.CheckEmailExists(register.Email))
            {
                ModelState.AddModelError("Email", "E-mail already exists");
                return View(register);
            }

            var user = _userRepo.CreateUser(
                register.Username,
                register.FirstName,
                register.LastName,
                register.Email,
                register.Password,
                register.CountryId);

            _mailService.Send(user.Email, user.SecurityToken);

            return RedirectToAction("UserRegistered");
        }
        public IActionResult UserRegistered()
        {
            return View();
        }
        public IActionResult Delete(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var blUser = _userRepo.GetUser(id);

                if (blUser == null)
                {
                    ModelState.AddModelError("Id", "User with specified ID doesn't exist");
                    return RedirectToAction("Users");
                }

                return View(blUser);
            }
            return View("Login");
        }


        [HttpPost]
        public IActionResult Delete(BLUser user)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                _userRepo.SoftDeleteUser(user.Id);

                return RedirectToAction("Users");
            }
            return View("Login");
        }

        public IActionResult ChangePass(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var blUser = _userRepo.GetUser(id);
                if (blUser == null)
                {
                    ModelState.AddModelError("Id", "User with specified ID doesn't exist");
                    return RedirectToAction("Users");
                }
                return View(blUser.Username);
            }
            return View("Login");
        }
        [HttpPost]
        public IActionResult ChangePass(VMChangePass changePassword)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                _userRepo.ChangePassword(changePassword.Username, changePassword.NewPassword);

                return RedirectToAction("Users");
            }
            return View("Login");

        }

        public IActionResult ValidateEmail(VMValidateMail validateEmail)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Login");

            _userRepo.ConfirmEmail(
                validateEmail.Email,
                validateEmail.SecurityToken);

            return RedirectToAction("Login");
        }
    }
}
