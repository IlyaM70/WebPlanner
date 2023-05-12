using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebPlanner.Models;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;

namespace WebPlanner.Controllers
{
    public class AccessController : Controller
    {
        private AppDbContext db;

        public AccessController(AppDbContext db)
        {
            this.db = db;
        }

        public IActionResult Login()
        {
            // проверка на аутентификация
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            if (claimsPrincipal.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            //если пользователь найден аутентификация
            var user = await db.Users.Where(u => u.Name == loginViewModel.Login).FirstOrDefaultAsync();
            if (user !=null && user.Password == loginViewModel.Password)
            {

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                     new Claim(ClaimTypes.Name, loginViewModel.Login)
                };

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {

                    AllowRefresh = true,
                    IsPersistent = loginViewModel.KeepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity), properties);

                return RedirectToAction("Index", "Home");
            };
            ViewData["ValidateMessage"] = "Пользователь не найден";
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            var existedUser = await db.Users.Where(u => u.Email == user.Email).FirstOrDefaultAsync();
            if (existedUser == null)
            {
                await db.Users.AddAsync(user);
                db.SaveChanges();
                ViewData["ValidateMessage"] = "Вы успешно зарегестрированы, теперь можете войти под вашим именем и паролем";
            }
            else
            {
                ViewData["ValidateMessage"] = "Уже есть пользователь с таким email";
            };

            return View();
        }

    }
}
