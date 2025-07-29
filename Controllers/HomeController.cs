using Employewebapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;

namespace Employewebapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Empdbcontext _context;
        public HomeController(ILogger<HomeController> logger, Empdbcontext context)
        {
            _logger = logger;
            _context = context;
        }

        

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult CreateEmployee(int? id)
        {
            if (id == null || id == 0)
            {
                // Return empty form for create
                return View(new CreateEmployee());
            }

            // Load employee for editing
            var emp = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (emp == null)
            {
                return NotFound();
            }

            return View(emp);
        }
    

        public IActionResult DeleteEmployee(int? Id)
        {
            var empobject=_context.Employees.FirstOrDefault(x => x.Id == Id);
            _context.Employees.Remove(empobject);
            _context.SaveChanges();
            return RedirectToAction("Employe");
        }


        [HttpPost]
        [HttpPost]
        public IActionResult CreateEmployeeForm(CreateEmployee emp)
        {
            if (ModelState.IsValid)
            {
                var existingEmp = _context.Employees.FirstOrDefault(x => x.Id == emp.Id);

                if (existingEmp == null)
                {
                    // Add new
                    _context.Employees.Add(emp);
                }
                else
                {
                    // Update existing
                    existingEmp.Name = emp.Name;
                    existingEmp.Phone = emp.Phone;
                    existingEmp.Salary = emp.Salary;
                    existingEmp.Description = emp.Description;

                    _context.Employees.Update(existingEmp);
                }

                _context.SaveChanges();
                return RedirectToAction("Employe");
            }

            return View("CreateEmployee", emp);
        }

        public IActionResult Employe()
        {
    //        List<Employe> emplist = new List<Employe>
    //        {
    //new Employe { Name = "Sai", Id = 20, Phone = 709382, Salary = 50000, Description = "Senior software" },
    //new Employe { Name = "Reyansh", Id = 21, Phone = 709381, Salary = 60000, Description = "Manager"  },
    //new Employe { Name = "Gopi", Id = 22, Phone = 709383, Salary = 70000, Description = "Tech lead"  }
    //    };

            var emplist = _context.Employees.ToList();
            // emp = new Employe();
            //emp.Id = 20;
            //emp.Name = "Sai";
            //emp.Salary = 2000000;
            //emp.Phone = 891962;
            //emp.Description = "Senior software engineer";
            return View(emplist);
        }
       
        
    }
}
    

