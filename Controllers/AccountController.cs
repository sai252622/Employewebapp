using Employewebapp.Models;
using Microsoft.AspNetCore.Mvc;

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
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password
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
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the user exists in the DB
                var user = _context.Users
                    .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    TempData["Message"] = "Login successful!";
                    return RedirectToAction("welcome");  // Or your employee dashboard
                }
                else
                {
                    TempData["Error"] = "Invalid email or password. Please try again.";
                    return View(model);
                }
            }

            return View(model);
        }

        public IActionResult Welcome()
        {
            return View();
        }


    }
}

