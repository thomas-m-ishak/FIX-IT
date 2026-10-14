using Microsoft.AspNetCore.Mvc;
using FIX_IT.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace FIX_IT.Controllers
{
    public class AccountController : Controller
    {
        private readonly FixItDBContext _context;

        public AccountController(FixItDBContext context)
        {
            _context = context;
        }

        [HttpGet] public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            ModelState.Remove("ReportedIssues");
            if (ModelState.IsValid)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.IsAdmin = !await _context.Users.AnyAsync();
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                await SignInUser(user);
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        [HttpGet] public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User loginUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginUser.Email);
            if (user != null && BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password))
            {
                await SignInUser(user);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Invalid Email or Password.");
            return View(loginUser);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "Client")
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }
    }
}
