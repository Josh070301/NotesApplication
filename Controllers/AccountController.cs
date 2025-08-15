using NotesApplication.Helpers;
using NotesApplication.Interfaces;
using NotesApplication.Models;
using NotesApplication.ViewModels;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace NotesApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_userRepository.GetByEmail(model.Email) != null)
                {
                    ModelState.AddModelError("", "Email already in use");
                    return View(model);
                }

                var passwordHash = HashPassword(model.Password);
                
                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = passwordHash,
                    CreatedAt = DateTime.Now
                };

                _userRepository.Add(user);
                
                return RedirectToAction("Login");
            }

            return View(model);
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.GetByEmail(model.Email);
                
                if (user != null && VerifyPassword(model.Password, user.PasswordHash))
                {
                    var token = JwtHelper.GenerateToken(user.Id, user.Username, user.Email);
                    
                    // Store token in cookie for web app use
                    Response.Cookies.Add(new System.Web.HttpCookie("auth_token", token)
                    {
                        HttpOnly = true,
                        Expires = DateTime.Now.AddDays(7)
                    });

                    // Set the current user principal, refers to the security context of the user, which includes their identity (who they are) and their claims (such as user ID, username, email, roles, etc.).
                    var principal = JwtHelper.ValidateToken(token);
                    HttpContext.User = principal;
                    
                    return RedirectToAction("Index", "Notes");
                }
                
                ModelState.AddModelError("", "Invalid email or password");
            }
            
            return View(model);
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            var cookie = new System.Web.HttpCookie("auth_token")
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            Response.Cookies.Add(cookie);
            
            return RedirectToAction("Login");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash.Equals(storedHash);
        }
    }
}