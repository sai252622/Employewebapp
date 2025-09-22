using Employewebapp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Employeewebapp.Controllers
{
    public class AccountController : Controller
    {
        private readonly Empdbcontext _context;

        public AccountController(Empdbcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (_context.Users.Any(u => u.Name == model.Name))
            {
                ModelState.AddModelError("Name", "This name is already taken.");
            }

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "This email is already registered.");
            }

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    RoleId = model.IsAdmin ? 1:2
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                TempData["Message"] = "Registration successful!";
                return RedirectToAction("Register");
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.RoleId == 1 ? "Admin" : "User")
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }

                TempData["Error"] = "Invalid email or password.";
            }

            return View(model);
        }

        public IActionResult Welcome()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user == null)
            {
                TempData["Error"] = "Email not found.";
                return View(model);
            }

            user.ResetToken = Guid.NewGuid().ToString();
            user.ResetTokenExpiry = DateTime.Now.AddMinutes(15);
            _context.SaveChanges();

            // Send token via email (simulate here)
            //var resetLink = Url.Action("ResetPassword", "Account",
            //    new { email = user.Email, token = user.ResetToken },
            //    protocol: HttpContext.Request.Scheme);
            var resetLink = user.ResetToken;

            // For now, just show on screen
            TempData["Message"] = $"Reset link: {resetLink}";

            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.ResetToken == token && u.ResetTokenExpiry > DateTime.Now);
            if (user == null)
            {
                TempData["Error"] = "Invalid or expired token.";
                return RedirectToAction("ForgotPassword");
            }

            return View(new ResetPasswordViewModel { Email = email, Token = token });
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _context.Users.FirstOrDefault(u =>
                u.Email == model.Email &&
                u.ResetToken == model.Token &&
                u.ResetTokenExpiry > DateTime.Now);

            if (user == null)
            {
                TempData["Error"] = "Invalid or expired token.";
                return View(model);
            }

            user.Password = model.NewPassword; // Hash in real apps!
            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            _context.SaveChanges();

            TempData["Message"] = "Password reset successful. Please login.";
            return RedirectToAction("Login");
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(); // If using cookie auth
            return RedirectToAction("Login", "Account");
        }



    }
}

