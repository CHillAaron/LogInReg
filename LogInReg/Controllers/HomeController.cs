using LogInReg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LogInReg.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LoginDbContext _context;

        public HomeController(ILogger<HomeController> logger, LoginDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public User UserInDb()
        {
            return _context.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(User reg)
        {
            if (ModelState.IsValid)
            {
                if(_context.Users.Any(u => u.Email == reg.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                reg.Password = Hasher.HashPassword(reg, reg.Password);
                _context.Users.Add(reg);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", reg.UserId);
                return RedirectToAction("Success");
            }
            else
            {
                return View("Index");
            }
        }
        [HttpPost("Login")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if (ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmission.Email);
                // If no user exists with provided email
                if (userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Index");
                }

                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();

                // verify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);

                // result can be compared to 0 for failure
                if (result == 0)
                {
                    // handle failure (this should be similar to how "existing email" is handled)
                    return View("Index");

                }
                
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                return RedirectToAction("Success");
            }
            else
            {
                return View("Index");
            }
        }
        [HttpGet("Success")]
        public IActionResult Success()
        {
            User userInDb = UserInDb();
            if(userInDb == null)
            {
                return RedirectToAction("LogOut");
            }
            return View(userInDb);
        }
        [HttpGet("LogOut")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
